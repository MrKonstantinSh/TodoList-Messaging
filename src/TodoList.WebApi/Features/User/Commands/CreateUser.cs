using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using TodoList.WebApi.DataAccess;

namespace TodoList.WebApi.Features.User.Commands;

public sealed record CreateUserCommand(string FirstName, string LastName, string? Email) : IRequest<Result<User>>;

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<User>>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IValidator<User> _userValidator;

    public CreateUserHandler(IMapper mapper, AppDbContext context, IValidator<User> userValidator)
    {
        _mapper = mapper;
        _context = context;
        _userValidator = userValidator;
    }
    
    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        var validationResult = await _userValidator.ValidateAsync(user, cancellationToken).ConfigureAwait(false);

        if (!validationResult.IsValid)
        {
            return Result.Fail("Fail"); // TODO: Add validation pipeline.
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return Result.Ok(user);
    }
}