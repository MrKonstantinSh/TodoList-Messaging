using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.WebApi.Features.Users.Commands;
using TodoList.WebApi.Features.Users.Queries;

namespace TodoList.WebApi.Features.Users.Endpoints;

[Route("[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var query = new GetUsersQuery();
        return this.ToActionResult(await _mediator.Send(query));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto?>> GetById([FromRoute] Guid id)
    {
        var query = new GetUserByIdQuery(id);
        return this.ToActionResult(await _mediator.Send(query));
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(request.FirstName, request.LastName, request.Email);
        return this.ToActionResult(await _mediator.Send(command));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDto?>> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(id, request.FirstName, request.LastName, request.Email);
        return this.ToActionResult(await _mediator.Send(command));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
    {
        var command = new DeleteUserCommand(id);
        return this.ToActionResult(await _mediator.Send(command));
    }
}