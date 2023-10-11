using Ardalis.Result;
using AutoMapper;
using MediatR;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.Users.Commands;

public sealed record CreateUserCommand(string FirstName, string LastName, string? Email) : IRequest<Result<User>>;

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<User>>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public CreateUserHandler(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return Result.Success(user);
    }
}