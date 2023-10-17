namespace TodoList.WebApi.Features.Users;

public sealed record UserDto(Guid Id, string FirstName, string LastName, string? Email)
{
    public UserDto(User user) : this(user.Id, user.FirstName, user.LastName, user.Email)
    {
    }
}