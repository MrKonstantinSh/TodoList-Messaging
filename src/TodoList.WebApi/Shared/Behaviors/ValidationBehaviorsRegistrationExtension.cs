using Ardalis.Result;
using MediatR;
using TodoList.WebApi.Features.Todos;
using TodoList.WebApi.Features.Todos.Commands;
using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Features.Users.Commands;

namespace TodoList.WebApi.Shared.Behaviors;

public static class ValidationBehaviorsRegistrationExtension
{
    public static MediatRServiceConfiguration RegisterValidationBehaviors(this MediatRServiceConfiguration config)
    {
        config.AddBehavior(typeof(IPipelineBehavior<CreateUserCommand, Result<UserDto>>),
            typeof(ValidationBehavior<CreateUserCommand, UserDto>));
        config.AddBehavior(typeof(IPipelineBehavior<UpdateUserCommand, Result<UserDto?>>),
            typeof(ValidationBehavior<UpdateUserCommand, UserDto?>));
        
        config.AddBehavior(typeof(IPipelineBehavior<CreateTodoCommand, Result<Todo>>),
            typeof(ValidationBehavior<CreateTodoCommand, Todo>));

        return config;
    }
}