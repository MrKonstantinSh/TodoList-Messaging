using TodoList.WebApi.Features.Users;

namespace TodoList.WebApi.UnitTests.Features;

public static class UserUtils
{
    public const string TestUserFirstId = "b219e131-38bd-4f15-81e4-59168d9215fd";
    public const string TestUserSecondId = "77202873-b224-47e5-b7fa-6f195b8c3d98";
    public const string TestUserThirdId = "186e09dc-6aa7-4fd1-939f-88787704b0e3";
    public const string TestUserNonExistentId = "4a75e9c6-6271-420e-ac7c-6d0ec319f007";
    
    public static IEnumerable<User> GetMockUserList()
    {
        return new List<User>
        {
            new("TestFirstNameFirst", "TestLastNameFirst")
            {
                Id = Guid.Parse(TestUserFirstId),
                Email = "test1@gmail.com"
            },
            new("TestFirstNameSecond", "TestLastNameSecond")
            {
                Id = Guid.Parse(TestUserSecondId),
                Email = "test2@gmail.com"
            },
            new("TestFirstNameThird", "TestLastNameThird")
            {
                Id = Guid.Parse(TestUserThirdId),
                Email = null
            }
        };
    }
}