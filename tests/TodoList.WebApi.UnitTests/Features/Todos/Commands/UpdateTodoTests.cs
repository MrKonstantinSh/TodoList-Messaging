using Ardalis.Result;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.MessagingContracts.Commands;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Todos.Commands;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Todos.Commands;

public sealed class UpdateTodoTests
{
    private readonly IConfiguration _mockConfiguration;
    private readonly AppDbContext _mockContext;
    private readonly ISendEndpointProvider _mockSendEndpointProvider;
    private readonly ISendEndpoint _mockSendEndpoint;

    public UpdateTodoTests()
    {
        var testConfiguration = new Dictionary<string, string>
        {
            {"RabbitMQ:SendEmailQueue", "send-email"},
        };

        _mockConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration!)
            .Build();
        
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        var usersMockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        var todosMockSet = TodoUtils.GetMockTodoList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(usersMockSet);
        _mockContext.Todos.Returns(todosMockSet);

        _mockSendEndpoint = Substitute.For<ISendEndpoint>();
        _mockSendEndpointProvider = Substitute.For<ISendEndpointProvider>();
        _mockSendEndpointProvider.GetSendEndpoint(Arg.Any<Uri>()).Returns(Task.FromResult(_mockSendEndpoint));
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.UpdateTodoCommandIsExecutedSuccessfullyWithoutSendingMessageDataSources),
        MemberType = typeof(TodoTestDataSources))]
    public async Task Update_todo_command_is_executed_successfully_without_sending_message(Guid todoId, string title,
        string description, string status, ICollection<Guid> assignedUserIds, TodoDto? expectedResult)
    {
        // Arrange
        var updateTodoCommand = new UpdateTodoCommand(todoId, title, description, status, assignedUserIds);
        var sut = new UpdateTodoHandler(_mockConfiguration, _mockContext, _mockSendEndpointProvider);
        
        // Act
        var commandResult = await sut.Handle(updateTodoCommand, new CancellationToken());
        
        // Assert
        _mockContext.Todos.Received().Update(Arg.Any<Todo>());
        await _mockContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        await _mockSendEndpoint.DidNotReceiveWithAnyArgs()
            .Send(Arg.Any<SendEmailCommand>(), Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeTrue();
        commandResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.UpdateTodoCommandIsExecutedSuccessfullyWithSendingMessageDataSources),
        MemberType = typeof(TodoTestDataSources))]
    public async Task Update_todo_command_is_executed_successfully_with_sending_message(Guid todoId, string title,
        string description, string status, ICollection<Guid> assignedUserIds, TodoDto? expectedResult)
    {
        // Arrange
        var updateTodoCommand = new UpdateTodoCommand(todoId, title, description, status, assignedUserIds);
        var sut = new UpdateTodoHandler(_mockConfiguration, _mockContext, _mockSendEndpointProvider);
        
        // Act
        var commandResult = await sut.Handle(updateTodoCommand, new CancellationToken());
        
        // Assert
        _mockContext.Todos.Received().Update(Arg.Any<Todo>());
        await _mockContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

        await _mockSendEndpoint.Received()
            .Send(Arg.Any<SendEmailCommand>(), Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeTrue();
        commandResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    
    [Theory]
    [MemberData(nameof(TodoTestDataSources.UpdateTodoCommandIfTodoIsNotExistReturnsNotFoundDataSources), MemberType = typeof(TodoTestDataSources))]
    public async Task Update_todo_command_if_todo_id_is_not_exist_returns_not_found(Guid todoId, string title,
        string description, string status, ICollection<Guid> assignedUserIds)
    {
        // Arrange
        var updateTodoCommand = new UpdateTodoCommand(todoId, title, description, status, assignedUserIds);
        var sut = new UpdateTodoHandler(_mockConfiguration, _mockContext, _mockSendEndpointProvider);
        
        // Act
        var commandResult = await sut.Handle(updateTodoCommand, new CancellationToken());
        
        // Assert
        _mockContext.Todos.DidNotReceiveWithAnyArgs().Update(Arg.Any<Todo>());
        await _mockContext.DidNotReceiveWithAnyArgs().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        await _mockSendEndpoint.DidNotReceiveWithAnyArgs()
            .Send(Arg.Any<SendEmailCommand>(), Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeFalse();
        commandResult.Status.Should().Be(ResultStatus.NotFound);
    }
}