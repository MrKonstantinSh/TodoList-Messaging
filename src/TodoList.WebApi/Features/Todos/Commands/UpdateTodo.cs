using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Todos.Commands;

public sealed record UpdateTodoCommand(Guid Id, string Title, string Description, string Status,
    ICollection<Guid> AssignedUserIds) : IRequest<Result<TodoDto?>>;

public sealed class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, Result<TodoDto?>>
{
    private readonly AppDbContext _context;

    public UpdateTodoHandler(AppDbContext context)
    {
        _context = context;
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
            .ToListAsync(cancellationToken));
        
        existingTodo.AssignedUsers = resultAssignedUsers;
        
        _context.Update(existingTodo);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return Result<TodoDto?>.Success(new TodoDto(existingTodo));
    }
}