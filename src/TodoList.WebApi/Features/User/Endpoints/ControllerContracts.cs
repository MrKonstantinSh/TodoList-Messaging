namespace TodoList.WebApi.Features.User.Endpoints;

public sealed record CreateUserRequest(string FirstName, string LastName, string? Email);