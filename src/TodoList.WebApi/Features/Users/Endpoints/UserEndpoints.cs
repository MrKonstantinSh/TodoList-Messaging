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
    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var query = new GetUsersQuery();
        return this.ToActionResult(await _mediator.Send(query));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<UserDto?>> GetById([FromRoute] Guid id)
    {
        var query = new GetUserByIdQuery(id);
        return this.ToActionResult(await _mediator.Send(query));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(request.FirstName, request.LastName, request.Email);
        return this.ToActionResult(await _mediator.Send(command));
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<UserDto?>> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(id, request.FirstName, request.LastName, request.Email);
        return this.ToActionResult(await _mediator.Send(command));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
    {
        var command = new DeleteUserCommand(id);
        return this.ToActionResult(await _mediator.Send(command));
    }
}