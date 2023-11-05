using Ardalis.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Todos.Commands;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Todos.Commands;

public sealed class DeleteTodoTests
{
    private readonly AppDbContext _mockContext;

    public DeleteTodoTests()
    {
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        
        var usersMockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        var todosMockSet = TodoUtils.GetMockTodoList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(usersMockSet);
        _mockContext.Todos.Returns(todosMockSet);
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.DeleteTodoCommandIsExecutedSuccessfullyDataSources), MemberType = typeof(TodoTestDataSources))]
    public async Task Delete_todo_command_is_executed_successfully(Guid todoId)
    {
        // Arrange
        var deleteTodoCommand = new DeleteTodoCommand(todoId);
        var sut = new DeleteTodoHandler(_mockContext);
        
        // Act
        var commandResult = await sut.Handle(deleteTodoCommand, new CancellationToken());
        
        // Assert
        _mockContext.Todos.Received().Remove(Arg.Any<Todo>());
        await _mockContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeTrue();
        commandResult.Value.Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.DeleteTodoCommandIfTodoIdIsNotExistReturnsNotFoundDataSources), MemberType = typeof(TodoTestDataSources))]
    public async Task Delete_user_command_if_user_id_is_not_exist_returns_not_found(Guid userId)
    {
        // Arrange
        var deleteTodoCommand = new DeleteTodoCommand(userId);
        var sut = new DeleteTodoHandler(_mockContext);
        
        // Act
        var commandResult = await sut.Handle(deleteTodoCommand, new CancellationToken());
        
        // Assert
        _mockContext.Todos.DidNotReceiveWithAnyArgs().Remove(Arg.Any<Todo>());
        await _mockContext.DidNotReceiveWithAnyArgs().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeFalse();
        commandResult.Status.Should().Be(ResultStatus.NotFound);
    }
}