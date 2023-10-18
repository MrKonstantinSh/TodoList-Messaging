using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Queries;

public sealed record GetUsersQuery : IRequest<Result<IEnumerable<UserDto>>>;

public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly AppDbContext _context;

    public GetUsersHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result<IEnumerable<UserDto>>.Success(users.Select(u => new UserDto(u)));
    }
}