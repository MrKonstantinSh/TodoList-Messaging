using Ardalis.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Todos.Queries;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Todos.Queries;

public sealed class GetTodoByIdTests
{
    private readonly AppDbContext _mockContext;

    public GetTodoByIdTests()
    {
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        
        var usersMockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        var todosMockSet = TodoUtils.GetMockTodoList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(usersMockSet);
        _mockContext.Todos.Returns(todosMockSet);
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.GetTodoByIdQueryIsExecutedSuccessfullyDataSources), MemberType = typeof(TodoTestDataSources))]
    public async Task Get_todo_by_id_query_is_executed_successfully(Guid todoId, TodoDto expectedUser)
    {
        // Arrange
        var getTodoByIdQuery = new GetTodoByIdQuery(todoId);
        var sut = new GetTodoByIdHandler(_mockContext);
        
        // Act
        var queryResult = await sut.Handle(getTodoByIdQuery, new CancellationToken());
        
        // Assert
        queryResult.IsSuccess.Should().BeTrue();
        queryResult.Value.Should().BeEquivalentTo(expectedUser);
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.GetTodoByIdQueryIfTodoIdIsNotExistReturnsNotFoundDataSources), MemberType = typeof(TodoTestDataSources))]
    public async Task Get_todo_by_id_query_if_todo_id_is_not_exist_returns_not_found(Guid userId)
    {
        // Arrange
        var getTodoByIdQuery = new GetTodoByIdQuery(userId);
        var sut = new GetTodoByIdHandler(_mockContext);
        
        // Act
        var queryResult = await sut.Handle(getTodoByIdQuery, new CancellationToken());
        
        // Assert
        queryResult.IsSuccess.Should().BeFalse();
        queryResult.Status.Should().Be(ResultStatus.NotFound);
    }
}