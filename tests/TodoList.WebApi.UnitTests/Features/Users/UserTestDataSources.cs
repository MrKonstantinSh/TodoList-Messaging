using TodoList.WebApi.Features.Users;

namespace TodoList.WebApi.UnitTests.Features.Users;

public static class UserTestDataSources
{
    public static IEnumerable<object?[]> CreateUserCommandIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                "TestFirstName",
                "TestLastName",
                "test1@gmail.com",
            };
            
            yield return new object?[]
            {
                "TestFirstName",
                "TestLastName",
                null,
            };
        }
    }
    
    public static IEnumerable<object?[]> UpdateUserCommandIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(UserUtils.TestUserFirstId),
                "TestFirstNameFirstUpdated",
                "TestLastNameFirstUpdated",
                "test1updated@gmail.com",
                new UserDto(Guid.Parse(UserUtils.TestUserFirstId), "TestFirstNameFirstUpdated",
                    "TestLastNameFirstUpdated", "test1updated@gmail.com")
            };
            
            yield return new object?[]
            {
                Guid.Parse(UserUtils.TestUserSecondId),
                "TestFirstNameSecondUpdated",
                "TestLastNameSecondUpdated",
                null,
                new UserDto(Guid.Parse(UserUtils.TestUserSecondId), "TestFirstNameSecondUpdated",
                    "TestLastNameSecondUpdated", null)
            };
        }
    }
    
    public static IEnumerable<object?[]> UpdateUserCommandIfUserIdIsNotExistReturnsNotFoundDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(UserUtils.TestUserNonExistentId),
                "TestFirstNameFourthUpdated",
                "TestLastNameFourthUpdated",
                "test4updated@gmail.com",
            };
        }
    }
    
    public static IEnumerable<object?[]> DeleteUserCommandIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(UserUtils.TestUserThirdId),
            };
        }
    }
    
    public static IEnumerable<object?[]> DeleteUserCommandIfUserIdIsNotExistReturnsNotFoundDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(UserUtils.TestUserNonExistentId),
            };
        }
    }
    
    public static IEnumerable<object?[]> GetUsersQueryIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                new List<UserDto>
                {
                    new(Guid.Parse(UserUtils.TestUserFirstId), "TestFirstNameFirst", "TestLastNameFirst", "test1@gmail.com"),
                    new(Guid.Parse(UserUtils.TestUserSecondId), "TestFirstNameSecond", "TestLastNameSecond", "test2@gmail.com"),
                    new(Guid.Parse(UserUtils.TestUserThirdId), "TestFirstNameThird", "TestLastNameThird", null)
                }
            };
        }
    }
    
    public static IEnumerable<object?[]> GetUserByIdQueryIsExecutedSuccessfullyDataSources
    {
        get
        {
            yield return new object[]
            {
                UserUtils.TestUserSecondId,
                new UserDto(Guid.Parse(UserUtils.TestUserSecondId), "TestFirstNameSecond", "TestLastNameSecond", "test2@gmail.com"),
            };
        }
    }
    
    public static IEnumerable<object?[]> GetUserByIdQueryIfUserIdIsNotExistReturnsNotFoundDataSources
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse(UserUtils.TestUserNonExistentId),
            };
        }
    }
}