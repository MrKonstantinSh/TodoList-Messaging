using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Users;

namespace TodoList.WebApi.UnitTests.Features;

public static class TodoUtils
{
    public const string TestTodoFirstId = "b219e131-38bd-4f15-81e4-59168d9215fd";
    public const string TestTodoSecondId = "77202873-b224-47e5-b7fa-6f195b8c3d98";
    public const string TestTodoNonExistentId = "4a75e9c6-6271-420e-ac7c-6d0ec319f007";
    
    public static IEnumerable<Todo> GetMockTodoList()
    {
        return new List<Todo>
        {
            new("Test title 1", "Test description 1", "To Do")
            {
                Id = Guid.Parse(TestTodoFirstId),
                AssignedUsers = new List<User>
                {
                    new("TestFirstNameFirst", "TestLastNameFirst")
                    {
                        Id = Guid.Parse(UserUtils.TestUserFirstId),
                        Email = "test1@gmail.com"
                    },
                    new("TestFirstNameSecond", "TestLastNameSecond")
                    {
                        Id = Guid.Parse(UserUtils.TestUserSecondId),
                        Email = "test2@gmail.com"
                    },
                }
            },
            new("Test title 2", "Test description 2", "In Progress")
            {
                Id = Guid.Parse(TestTodoSecondId),
                AssignedUsers = new List<User>()
            }
        };
    }
}