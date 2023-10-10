using FluentResults.Extensions.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.WebApi.Features.User.Commands;

namespace TodoList.WebApi.Features.User.Endpoints;

[Route("[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(request.FirstName, request.LastName, request.Email);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }
}