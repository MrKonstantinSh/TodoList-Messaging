using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Queries;

public sealed record GetUsersQuery : IRequest<Result<IEnumerable<User>>>;

public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, Result<IEnumerable<User>>>
{
    private readonly AppDbContext _context;

    public GetUsersHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken).ConfigureAwait(false);

        return Result<IEnumerable<User>>.Success(users);
    }
}