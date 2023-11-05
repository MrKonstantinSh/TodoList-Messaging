using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Shared;

namespace TodoList.WebApi.Features.Users;

public sealed class User : EntityBase
{
    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        Todos = new List<Todo>();
    }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }

    public ICollection<Todo> Todos { get; set; }
}