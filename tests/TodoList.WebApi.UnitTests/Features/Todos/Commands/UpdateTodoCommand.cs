using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Users.Commands;

namespace TodoList.WebApi.UnitTests.Features.Todos.Commands;

public sealed class UpdateTodoCommand
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _mockContext;

    public UpdateTodoCommand()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
            cfg.AddMaps(Assembly.GetAssembly(typeof(CreateUserCommand))!.FullName));
        _mapper = mapperConfiguration.CreateMapper();
        
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
        var usersMockSet = UserUtils.GetMockUserList().AsQueryable().BuildMockDbSet();
        var todosMockSet = TodoUtils.GetMockTodoList().AsQueryable().BuildMockDbSet();
        _mockContext.Users.Returns(usersMockSet);
        _mockContext.Todos.Returns(todosMockSet);
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
}