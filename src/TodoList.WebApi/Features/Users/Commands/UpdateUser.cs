using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Commands;

public sealed record UpdateUserCommand(Guid Id, string FirstName, string LastName, string? Email)
    : IRequest<Result<UserDto?>>;

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto?>>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public UpdateUserHandler(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result<UserDto?>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        var existingUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken)
            .ConfigureAwait(false);
        
        if (existingUser is null)
        {
            return Result<UserDto?>.NotFound();
        }
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return Result<UserDto?>.Success(new UserDto(user));
    }
}