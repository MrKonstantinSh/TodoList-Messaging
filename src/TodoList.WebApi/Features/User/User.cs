using TodoList.WebApi.Shared;

namespace TodoList.WebApi.Features.User;

public sealed class User : EntityBase
{
    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
}