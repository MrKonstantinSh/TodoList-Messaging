using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Todos.Queries;

public sealed record GetTodosQuery : IRequest<Result<IEnumerable<TodoDto>>>;

public sealed class GetTodosHandler : IRequestHandler<GetTodosQuery, Result<IEnumerable<TodoDto>>>
{
    private readonly AppDbContext _context;

    public GetTodosHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<IEnumerable<TodoDto>>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var todos = await _context.Todos.Include(t => t.AssignedUsers)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result<IEnumerable<TodoDto>>.Success(todos.Select(t => new TodoDto(t)));
    }
}