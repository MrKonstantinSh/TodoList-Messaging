using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Todos.Queries;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Todos.Queries;

public sealed class GetTodosTests
{
    private readonly AppDbContext _mockContext;

    public GetTodosTests()
    {
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        
        var usersMockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        var todosMockSet = TodoUtils.GetMockTodoList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(usersMockSet);
        _mockContext.Todos.Returns(todosMockSet);
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.GetTodosQueryIsExecutedSuccessfullyDataSources), MemberType = typeof(TodoTestDataSources))]
    public async Task Get_todos_query_is_executed_successfully(IEnumerable<TodoDto> expectedUsers)
    {
        // Arrange
        var getTodosQuery = new GetTodosQuery();
        var sut = new GetTodosHandler(_mockContext);
        
        // Act
        var queryResult = await sut.Handle(getTodosQuery, new CancellationToken());
        
        // Assert
        queryResult.IsSuccess.Should().BeTrue();
        queryResult.Value.Should().BeEquivalentTo(expectedUsers);
    }
}