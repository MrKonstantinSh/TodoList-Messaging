using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Queries;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<Result<User?>>;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<User?>>
{
    private readonly AppDbContext _context;

    public GetUserByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);

        return user is null ? Result<User?>.NotFound() : Result<User?>.Success(user);
    }
}