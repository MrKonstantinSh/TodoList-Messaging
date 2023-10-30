using System.Reflection;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Todos.Commands;
using TodoList.WebApi.Features.Users.Commands;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Todos.Commands;

public sealed class CreateTodoTests
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _mockContext;

    public CreateTodoTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
            cfg.AddMaps(Assembly.GetAssembly(typeof(CreateUserCommand))!.FullName));
        _mapper = mapperConfiguration.CreateMapper();
        
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        var usersMockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(usersMockSet);
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.CreateTodoCommandIsExecutedSuccessfullyDataSources), MemberType = typeof(TodoTestDataSources))]
    public async Task Create_todo_command_is_executed_successfully(string title, string description, string status,
        ICollection<Guid> assignedUserIds, TodoDto expectedTodo)
    {
        // Arrange
        var createTodoCommand = new CreateTodoCommand(title, description, status, assignedUserIds);
        var sut = new CreateTodoHandler(_mapper, _mockContext);
        
        // Act
        var commandResult = await sut.Handle(createTodoCommand, new CancellationToken());
        
        // Assert
        _mockContext.Todos.Received().Add(Arg.Any<Todo>());
        await _mockContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeTrue();
        commandResult.Value.Should().BeEquivalentTo(expectedTodo);
    }
}