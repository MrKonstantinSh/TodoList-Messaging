using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Commands;

public sealed record DeleteUserCommand(Guid Id) : IRequest<Result<bool>>;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
{
    private readonly AppDbContext _context;

    public DeleteUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userToDelete = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (userToDelete is null)
        {
            return Result<bool>.NotFound();
        }

        _context.Users.Remove(userToDelete);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return Result<bool>.Success(true);
    }
}