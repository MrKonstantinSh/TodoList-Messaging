using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Users.Commands;

namespace TodoList.WebApi.Features.Users.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator(AppDbContext context)
    {
        RuleFor(u => u.FirstName)
            .NotNull().WithMessage($"The {nameof(User.FirstName)} must not be null.")
            .NotEmpty().WithMessage($"The {nameof(User.FirstName)} must not be empty.")
            .MaximumLength(250).WithMessage($"The {nameof(User.FirstName)} must not exceed 250 characters in length.")
            .Matches("^[a-zA-Z]*$").WithMessage($"The {nameof(User.FirstName)} must contain only letters from A-Z, a-z.");

        RuleFor(u => u.LastName)
            .NotNull().WithMessage($"The {nameof(User.LastName)} must not be null.")
            .NotEmpty().WithMessage($"The {nameof(User.LastName)} must not be empty.")
            .MaximumLength(250).WithMessage($"The {nameof(User.LastName)} must not exceed 250 characters in length.")
            .Matches("^[a-zA-Z]*$").WithMessage($"The {nameof(User.LastName)} must contain only letters from A-Z, a-z.");

        RuleFor(u => u.Email)
            .EmailAddress().When(u => !string.IsNullOrEmpty(u.Email)).WithMessage($"The {nameof(User.Email)} is not correct.")
            .MustAsync(async (email, cancellationToken) =>
            {
                var user = await context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                    .ConfigureAwait(false);

                return user is null;
            }).WithMessage($"A user with this {nameof(User.Email)} already exists.");
    }
}