using Ardalis.Result;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Messaging.Commands;

namespace TodoList.WebApi.Features.Todos.Commands;

public sealed record UpdateTodoCommand(Guid Id, string Title, string Description, string Status,
    ICollection<Guid> AssignedUserIds) : IRequest<Result<TodoDto?>>;

public sealed class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, Result<TodoDto?>>
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public UpdateTodoHandler(
        IConfiguration configuration,
        AppDbContext context,
        ISendEndpointProvider sendEndpointProvider)
    {
        _configuration = configuration;
        _context = context;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task<Result<TodoDto?>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var existingTodo = await _context.Todos
            .Include(t => t.AssignedUsers)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);
        
        if (existingTodo is null)
        {
            return Result<TodoDto?>.NotFound();
        }

        var initialTodoStatus = existingTodo.Status;
        
        existingTodo.Title = request.Title;
        existingTodo.Description = request.Description;
        existingTodo.Status = request.Status;
        
        var assignedUserIds = existingTodo.AssignedUsers.Select(u => u.Id).ToList();
        var updatedAssignedUserIds = await _context.Users
            .Where(u => request.AssignedUserIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var userIdsToRemove = assignedUserIds.Except(updatedAssignedUserIds).ToList();
        var userIdsToAdd = updatedAssignedUserIds.Except(assignedUserIds).ToList();

        var resultAssignedUsers = existingTodo.AssignedUsers.ToList();
        userIdsToRemove.ForEach(id => resultAssignedUsers.Remove(resultAssignedUsers.First(u => u.Id == id)));
        resultAssignedUsers.AddRange(await _context.Users
            .AsNoTracking()
            .Where(u => userIdsToAdd.Contains(u.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false));
        
        existingTodo.AssignedUsers = resultAssignedUsers;
        
        _context.Update(existingTodo);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        // Send message to message queue if status has changed.
        if (!string.Equals(initialTodoStatus, existingTodo.Status, StringComparison.Ordinal))
        {
            var emails = existingTodo.AssignedUsers
                .Where(u => !string.IsNullOrEmpty(u.Email))
                .Select(u => u.Email!)
                .ToList();
            var message = $"Task status '{existingTodo.Title}' changed from '{initialTodoStatus}' to '{existingTodo.Status}'.";

            await SendEmailCommandsToMessageQueueAsync(new SendEmailCommand(message, emails), cancellationToken)
                .ConfigureAwait(false);
        }
        
        return Result<TodoDto?>.Success(new TodoDto(existingTodo));
    }

    private async Task SendEmailCommandsToMessageQueueAsync(SendEmailCommand sendEmailCommand,CancellationToken cancellationToken)
    {
        var endpoint = await _sendEndpointProvider
            .GetSendEndpoint(new Uri($"queue:{_configuration["RabbitMQ:SendEmailQueue"]}"))
            .ConfigureAwait(false);

        await endpoint.Send(sendEmailCommand, cancellationToken).ConfigureAwait(false);
    }
}