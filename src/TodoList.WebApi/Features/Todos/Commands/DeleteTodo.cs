using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Todos.Commands;

public sealed record DeleteTodoCommand(Guid Id) : IRequest<Result<bool>>;

public sealed class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand, Result<bool>>
{
    private readonly AppDbContext _context;

    public DeleteTodoHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var todoToDelete = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (todoToDelete is null)
        {
            return Result<bool>.NotFound();
        }

        _context.Todos.Remove(todoToDelete);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return Result<bool>.Success(true);
    }
}