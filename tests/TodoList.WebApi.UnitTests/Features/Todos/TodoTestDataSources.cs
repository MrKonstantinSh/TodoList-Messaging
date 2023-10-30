using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Users;

namespace TodoList.WebApi.UnitTests.Features.Todos;

public static class TodoTestDataSources
{
    public static IEnumerable<object?[]> CreateTodoCommandIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                "Test title 1",
                "Test description 1",
                "To Do",
                new List<Guid>
                {
                    Guid.Parse(UserUtils.TestUserFirstId)
                },
                new TodoDto(Guid.Empty, "Test title 1", "Test description 1", "To Do",
                    new List<UserDto>
                    {
                        new(Guid.Parse(UserUtils.TestUserFirstId), "TestFirstNameFirst", "TestLastNameFirst", "test1@gmail.com")
                    })
            };
            
            yield return new object[]
            {
                "Test title 2",
                "Test description 2",
                "",
                new List<Guid>
                {
                    Guid.Parse(UserUtils.TestUserFirstId),
                    Guid.Parse(UserUtils.TestUserSecondId),
                },
                new TodoDto(Guid.Empty, "Test title 2", "Test description 2", Todo.InitialValue,
                    new List<UserDto>
                    {
                        new(Guid.Parse(UserUtils.TestUserFirstId), "TestFirstNameFirst", "TestLastNameFirst", "test1@gmail.com"),
                        new(Guid.Parse(UserUtils.TestUserSecondId), "TestFirstNameSecond", "TestLastNameSecond", "test2@gmail.com")
                    })
            };
            
            yield return new object[]
            {
                "Test title 3",
                "Test description 3",
                "In Progress",
                new List<Guid>(),
                new TodoDto(Guid.Empty, "Test title 3", "Test description 3", "In Progress", new List<UserDto>())
            };
        }
    }
}