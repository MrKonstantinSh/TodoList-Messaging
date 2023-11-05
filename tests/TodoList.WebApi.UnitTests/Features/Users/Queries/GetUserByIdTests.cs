using Ardalis.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Features.Users.Queries;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Users.Queries;

public sealed class GetUserByIdTests
{
    private readonly AppDbContext _mockContext;
    
    public GetUserByIdTests()
    {
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        
        var mockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(mockSet);
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.GetUserByIdQueryIsExecutedSuccessfullyDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Get_user_by_id_query_is_executed_successfully(Guid userId, UserDto expectedUser)
    {
        // Arrange
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var sut = new GetUserByIdHandler(_mockContext);
        
        // Act
        var queryResult = await sut.Handle(getUserByIdQuery, new CancellationToken());
        
        // Assert
        queryResult.IsSuccess.Should().BeTrue();
        queryResult.Value.Should().BeEquivalentTo(expectedUser);
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.GetUserByIdQueryIfUserIdIsNotExistReturnsNotFoundDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Get_user_by_id_query_if_user_id_is_not_exist_returns_not_found(Guid userId)
    {
        // Arrange
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var sut = new GetUserByIdHandler(_mockContext);
        
        // Act
        var queryResult = await sut.Handle(getUserByIdQuery, new CancellationToken());
        
        // Assert
        queryResult.IsSuccess.Should().BeFalse();
        queryResult.Status.Should().Be(ResultStatus.NotFound);
    }
}