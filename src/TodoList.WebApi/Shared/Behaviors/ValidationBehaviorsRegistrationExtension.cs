using Ardalis.Result;
using MediatR;
using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Features.Users.Commands;

namespace TodoList.WebApi.Shared.Behaviors;

public static class ValidationBehaviorsRegistrationExtension
{
    public static MediatRServiceConfiguration RegisterValidationBehaviors(this MediatRServiceConfiguration config)
    {
        config.AddBehavior(typeof(IPipelineBehavior<CreateUserCommand, Result<User>>),
            typeof(ValidationBehavior<CreateUserCommand, User>));
        config.AddBehavior(typeof(IPipelineBehavior<UpdateUserCommand, Result<User?>>),
            typeof(ValidationBehavior<UpdateUserCommand, User?>));

        return config;
    }
}