using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Queries;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto?>>;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto?>>
{
    private readonly AppDbContext _context;

    public GetUserByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);

        return user is null ? Result<UserDto?>.NotFound() : Result<UserDto?>.Success(new UserDto(user));
    }
}