using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application;
using TodoApp.Application.Common;
using TodoApp.Application.Todos.Commands;
using TodoApp.Application.Todos.Dtos;
using TodoApp.Application.Todos.Queries;

namespace TodoApp.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[Authorize]
public sealed class TodosController : ControllerBase
{
    private readonly IMediator mediator;

    public TodosController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<TodoDto>))]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<TodoDto>> GetTodos(TodoStatusDto? status, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetTodos(status, page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<TodoDto>> GetTodoById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTodoById(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<TodoDto>> CreateTodo(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateTodo(request.Title, request.Description, request.Status), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetTodoById), new { id = data.Id }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteTodo(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTodo(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateTitle(int id, [FromBody] string title, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateTitle(id, title), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateDescription(int id, [FromBody] string? description, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateDescription(id, description), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateStatus(int id, [FromBody] TodoStatusDto status, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateStatus(id, status), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/estimatedHours")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateEstimatedHours(int id, [FromBody] double? hours, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateEstimatedHours(id, hours), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/remainingHours")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateRemainingHours(int id, [FromBody] double? hours, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateRemainingHours(id, hours), cancellationToken);
        return this.HandleResult(result);
    }
}
