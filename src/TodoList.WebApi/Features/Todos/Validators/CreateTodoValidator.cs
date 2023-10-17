using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Todos.Commands;
using TodoList.WebApi.Features.Users;

namespace TodoList.WebApi.Features.Todos.Validators;

public class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoValidator(AppDbContext context)
    {
        RuleFor(t => t.Title)
            .NotNull().WithMessage($"The {nameof(Todo.Title)} must not be null.")
            .NotEmpty().WithMessage($"The {nameof(Todo.Title)} must not be empty.")
            .MaximumLength(250).WithMessage($"The {nameof(Todo.Title)} must not exceed 250 characters in length.");

        RuleFor(t => t.Description)
            .NotNull().WithMessage($"The {nameof(Todo.Description)} must not be null.")
            .NotEmpty().WithMessage($"The {nameof(Todo.Description)} must not be empty.")
            .MaximumLength(10_000).WithMessage($"The {nameof(Todo.Description)} must not exceed 10000 characters in length.");

        RuleFor(t => t.Status)
            .MaximumLength(20).WithMessage($"The {nameof(Todo.Status)} must not exceed 20 characters in length.");

        RuleForEach(t => t.AssignedUserIds)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return user is not null;
            }).WithMessage((_, userId) => $"The user with id '{userId}' was not found.");
    }
}