namespace TodoList.WebApi.Features.Users.Endpoints;

public sealed record CreateUserRequest(string FirstName, string LastName, string? Email);

public sealed record UpdateUserRequest(string FirstName, string LastName, string? Email);
