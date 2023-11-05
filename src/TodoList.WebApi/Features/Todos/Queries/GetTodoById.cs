using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Todos.Queries;

public sealed record GetTodoByIdQuery(Guid Id) : IRequest<Result<TodoDto?>>;

public sealed class GetTodoByIdHandler : IRequestHandler<GetTodoByIdQuery, Result<TodoDto?>>
{
    private readonly AppDbContext _context;

    public GetTodoByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TodoDto?>> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos
            .Include(t => t.AssignedUsers)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);

        return todo is null ? Result<TodoDto?>.NotFound() : Result<TodoDto?>.Success(new TodoDto(todo));
    }
}