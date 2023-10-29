using System.Reflection;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Features.Users.Commands;
using Xunit;

namespace TodoList.WebApi.UnitTests.Features.Users.Commands;

public sealed class CreateUserTests
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _mockContext;

    public CreateUserTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
            cfg.AddMaps(Assembly.GetAssembly(typeof(CreateUserCommand))!.FullName));
        _mapper = mapperConfiguration.CreateMapper();
        
        _mockContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
    }
    
    [Theory]
    [MemberData(nameof(UserTestDataSources.CreateUserCommandIsExecutedSuccessfullyDataSources), MemberType = typeof(UserTestDataSources))]
    public async Task Create_user_command_is_executed_successfully(string firstName, string lastName, string? email)
    {
        // Arrange
        var createUserCommand = new CreateUserCommand(firstName, lastName, email);
        var sut = new CreateUserHandler(_mapper, _mockContext);
        
        // Act
        var commandResult = await sut.Handle(createUserCommand, new CancellationToken());
        
        // Assert
        _mockContext.Users.Received().Add(Arg.Any<User>());
        await _mockContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        
        commandResult.IsSuccess.Should().BeTrue();
        commandResult.Value.FirstName.Should().BeEquivalentTo(firstName);
        commandResult.Value.LastName.Should().BeEquivalentTo(lastName);
        commandResult.Value.Email.Should().BeEquivalentTo(email);
    }
}