using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.User;

public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator(AppDbContext context)
    {
        RuleFor(u => u.FirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(u => u.LastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(u => u.Email)
            .EmailAddress().When(u => !string.IsNullOrEmpty(u.Email))
            .MustAsync(async (email, cancellationToken) =>
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                    .ConfigureAwait(false);

                return user is null;
            });
    }
}