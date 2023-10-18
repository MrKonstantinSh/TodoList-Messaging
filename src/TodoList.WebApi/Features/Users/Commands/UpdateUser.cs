using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Commands;

public sealed record UpdateUserCommand(Guid Id, string FirstName, string LastName, string? Email)
    : IRequest<Result<UserDto?>>;

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto?>>
{
    private readonly AppDbContext _context;

    public UpdateUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDto?>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);
        
        if (existingUser is null)
        {
            return Result<UserDto?>.NotFound();
        }

        existingUser.FirstName = request.FirstName;
        existingUser.LastName = request.LastName;
        existingUser.Email = request.Email;
        
        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return Result<UserDto?>.Success(new UserDto(existingUser));
    }
}