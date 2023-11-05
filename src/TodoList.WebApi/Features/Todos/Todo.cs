using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Shared;

namespace TodoList.WebApi.Features.Todos;

public sealed class Todo : EntityBase
{
    public const string InitialValue = "To do";
    
    public Todo(string title, string description, string status)
    {
        Title = title;
        Description = description;
        Status = status;
    
        AssignedUsers = new List<User>();
    }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    
    public ICollection<User> AssignedUsers { get; set; }
}