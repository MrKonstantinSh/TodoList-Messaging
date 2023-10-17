using TodoList.WebApi.Features.Users;

namespace TodoList.WebApi.Features.Todos;

public sealed record TodoDto(Guid Id, string Title, string Description,
    string Status, ICollection<UserDto> AssignedUsers)
{
    public TodoDto(Todo todo) : this(todo.Id, todo.Title, todo.Description,
        todo.Status, todo.AssignedUsers.Select(u => new UserDto(u)).ToList())
    {
        
    }
}