using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Todos.Commands;

public sealed record CreateTodoCommand(string Title, string Description, string Status,
    ICollection<Guid> AssignedUserIds) : IRequest<Result<TodoDto>>;

public sealed class CreateTodoHandler : IRequestHandler<CreateTodoCommand, Result<TodoDto>>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public CreateTodoHandler(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result<TodoDto>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = _mapper.Map<Todo>(request);

        if (string.IsNullOrEmpty(todo.Status))
        {
            todo.Status = Todo.InitialValue;
        }

        var users = await _context.Users
            .Where(u => request.AssignedUserIds.Contains(u.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        todo.AssignedUsers = users;

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return Result<TodoDto>.Success(new TodoDto(todo));
    }
}