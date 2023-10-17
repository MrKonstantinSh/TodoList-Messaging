namespace TodoList.WebApi.Features.Todos.Endpoints;

public sealed record CreateTodoRequest(string Title, string Description,
    string Status, ICollection<Guid> AssignedUserIds);
