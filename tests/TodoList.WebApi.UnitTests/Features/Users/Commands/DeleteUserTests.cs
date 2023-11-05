using Ardalis.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Features.Users.Commands;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Users.Commands;

public sealed class DeleteUserTests
{
    private readonly AppDbContext _mockContext;
    
    public DeleteUserTests()
    {
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        
        var mockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(mockSet);
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.DeleteUserCommandIsExecutedSuccessfullyDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Delete_user_command_is_executed_successfully(Guid userId)
    {
        // Arrange
        var deleteUserCommand = new DeleteUserCommand(userId);
        var sut = new DeleteUserHandler(_mockContext);
        
        // Act
        var commandResult = await sut.Handle(deleteUserCommand, new CancellationToken());
        
        // Assert
        _mockContext.Users.Received().Remove(Arg.Any<User>());
        await _mockContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeTrue();
        commandResult.Value.Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.DeleteUserCommandIfUserIdIsNotExistReturnsNotFoundDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Delete_user_command_if_user_id_is_not_exist_returns_not_found(Guid userId)
    {
        // Arrange
        var deleteUserCommand = new DeleteUserCommand(userId);
        var sut = new DeleteUserHandler(_mockContext);
        
        // Act
        var commandResult = await sut.Handle(deleteUserCommand, new CancellationToken());
        
        // Assert
        _mockContext.Users.DidNotReceiveWithAnyArgs().Remove(Arg.Any<User>());
        await _mockContext.DidNotReceiveWithAnyArgs().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeFalse();
        commandResult.Status.Should().Be(ResultStatus.NotFound);
    }
}