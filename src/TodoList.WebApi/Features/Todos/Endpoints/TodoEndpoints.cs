using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.WebApi.Features.Todos.Commands;
using TodoList.WebApi.Features.Todos.Queries;

namespace TodoList.WebApi.Features.Todos.Endpoints;

[Route("[controller]")]
public sealed class TodosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodosController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAll()
    {
        var query = new GetTodosQuery();
        return this.ToActionResult(await _mediator.Send(query));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoDto?>> GetById([FromRoute] Guid id)
    {
        var query = new GetTodoByIdQuery(id);
        return this.ToActionResult(await _mediator.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create([FromBody] CreateTodoRequest request)
    {
        var command = new CreateTodoCommand(request.Title, request.Description,
            request.Status, request.AssignedUserIds);
        
        return this.ToActionResult(await _mediator.Send(command));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TodoDto?>> Update([FromRoute] Guid id, [FromBody] UpdateTodoRequest request)
    {
        var command = new UpdateTodoCommand(id, request.Title, request.Description,
            request.Status, request.AssignedUserIds);
        
        return this.ToActionResult(await _mediator.Send(command));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] Guid id)
    {
        var command = new DeleteTodoCommand(id);
        return this.ToActionResult(await _mediator.Send(command));
    }
}