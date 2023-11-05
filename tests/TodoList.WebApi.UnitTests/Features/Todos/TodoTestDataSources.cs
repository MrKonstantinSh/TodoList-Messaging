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

    public static IEnumerable<object?[]> UpdateTodoCommandIsExecutedSuccessfullyWithoutSendingMessageDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(TodoUtils.TestTodoFirstId),
                "Test title 1 updated",
                "Test description 1 updated",
                "To Do",
                new List<Guid>
                {
                    Guid.Parse(UserUtils.TestUserFirstId),
                    Guid.Parse(UserUtils.TestUserThirdId)
                },
                new TodoDto(Guid.Parse(TodoUtils.TestTodoFirstId), "Test title 1 updated", "Test description 1 updated", "To Do",
                    new List<UserDto>
                    {
                        new(Guid.Parse(UserUtils.TestUserFirstId), "TestFirstNameFirst", "TestLastNameFirst", "test1@gmail.com"),
                        new(Guid.Parse(UserUtils.TestUserThirdId), "TestFirstNameThird", "TestLastNameThird", null)
                    })
            };
        }
    }
    
    public static IEnumerable<object?[]> UpdateTodoCommandIsExecutedSuccessfullyWithSendingMessageDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(TodoUtils.TestTodoSecondId),
                "Test title 2 updated",
                "Test description 2 updated",
                "Done",
                new List<Guid>
                {
                    Guid.Parse(UserUtils.TestUserSecondId)
                },
                new TodoDto(Guid.Parse(TodoUtils.TestTodoSecondId), "Test title 2 updated", "Test description 2 updated", "Done",
                    new List<UserDto>
                    {
                        new(Guid.Parse(UserUtils.TestUserSecondId), "TestFirstNameSecond", "TestLastNameSecond", "test2@gmail.com"),
                    })
            };
        }
    }
    
    public static IEnumerable<object?[]> UpdateTodoCommandIfTodoIsNotExistReturnsNotFoundDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(TodoUtils.TestTodoNonExistentId),
                "Test title 4 updated",
                "Test description 4 updated",
                "Done",
                new List<Guid>
                {
                    Guid.Parse(UserUtils.TestUserThirdId)
                }
            };
        }
    }
    
    public static IEnumerable<object?[]> DeleteTodoCommandIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(TodoUtils.TestTodoFirstId),
            };
        }
    }
    
    public static IEnumerable<object?[]> DeleteTodoCommandIfTodoIdIsNotExistReturnsNotFoundDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(TodoUtils.TestTodoNonExistentId),
            };
        }
    }
    
    public static IEnumerable<object?[]> GetTodosQueryIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                new List<TodoDto>
                {
                    new(Guid.Parse(TodoUtils.TestTodoFirstId), "Test title 1", "Test description 1", "To Do",
                        new List<UserDto>
                        {
                            new(Guid.Parse(UserUtils.TestUserFirstId), "TestFirstNameFirst", "TestLastNameFirst", "test1@gmail.com"),
                            new(Guid.Parse(UserUtils.TestUserSecondId), "TestFirstNameSecond", "TestLastNameSecond", "test2@gmail.com")
                        }),
                    new(Guid.Parse(TodoUtils.TestTodoSecondId), "Test title 2", "Test description 2", "In Progress", new List<UserDto>()),
                }
            };
        }
    }
    
    public static IEnumerable<object?[]> GetTodoByIdQueryIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                TodoUtils.TestTodoFirstId,
                new TodoDto(Guid.Parse(TodoUtils.TestTodoFirstId), "Test title 1", "Test description 1", "To Do",
                    new List<UserDto> 
                    {
                        new(Guid.Parse(UserUtils.TestUserFirstId), "TestFirstNameFirst", "TestLastNameFirst", "test1@gmail.com"),
                        new(Guid.Parse(UserUtils.TestUserSecondId), "TestFirstNameSecond", "TestLastNameSecond", "test2@gmail.com")
                    })
            };
        }
    }
    
    public static IEnumerable<object?[]> GetTodoByIdQueryIfTodoIdIsNotExistReturnsNotFoundDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(TodoUtils.TestTodoNonExistentId),
            };
        }
    }
}