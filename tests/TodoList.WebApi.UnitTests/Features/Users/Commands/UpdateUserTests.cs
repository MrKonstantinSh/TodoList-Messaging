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

public sealed class UpdateUserTests
{
    private readonly AppDbContext _mockContext;

    public UpdateUserTests()
    {
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        
        var mockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(mockSet);
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.UpdateUserCommandIsExecutedSuccessfullyDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Update_user_command_is_executed_successfully(Guid userId, string newFirstName, string newLastName,
        string? newEmail, UserDto expectedResult)
    {
        // Arrange
        var updateUserCommand = new UpdateUserCommand(userId, newFirstName, newLastName, newEmail);
        var sut = new UpdateUserHandler(_mockContext);
        
        // Act
        var commandResult = await sut.Handle(updateUserCommand, new CancellationToken());
        
        // Assert
        _mockContext.Users.Received().Update(Arg.Any<User>());
        await _mockContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeTrue();
        commandResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.UpdateUserCommandIfUserIdIsNotExistReturnsNotFoundDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Update_user_command_if_user_id_is_not_exist_returns_not_found(Guid userId, string newFirstName, string newLastName,
        string? newEmail)
    {
        // Arrange
        var updateUserCommand = new UpdateUserCommand(userId, newFirstName, newLastName, newEmail);
        var sut = new UpdateUserHandler(_mockContext);
        
        // Act
        var commandResult = await sut.Handle(updateUserCommand, new CancellationToken());
        
        // Assert
        _mockContext.Users.DidNotReceiveWithAnyArgs().Update(Arg.Any<User>());
        await _mockContext.DidNotReceiveWithAnyArgs().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeFalse();
        commandResult.Status.Should().Be(ResultStatus.NotFound);
    }
}