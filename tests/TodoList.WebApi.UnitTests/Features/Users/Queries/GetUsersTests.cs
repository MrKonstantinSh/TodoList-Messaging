using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Features.Users.Queries;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Users.Queries;

public sealed class GetUsersTests
{
    private readonly AppDbContext _mockContext;
    
    public GetUsersTests()
    {
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        
        var mockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(mockSet);
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.GetUsersQueryIsExecutedSuccessfullyDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Get_users_query_is_executed_successfully(IEnumerable<UserDto> expectedUsers)
    {
        // Arrange
        var getUsersQuery = new GetUsersQuery();
        var sut = new GetUsersHandler(_mockContext);
        
        // Act
        var queryResult = await sut.Handle(getUsersQuery, new CancellationToken());
        
        // Assert
        queryResult.IsSuccess.Should().BeTrue();
        queryResult.Value.Should().BeEquivalentTo(expectedUsers);
    }
}