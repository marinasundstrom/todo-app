using TodoApp.Application;
using TodoApp.Application.Common;
using TodoApp.Application.Todos.Commands;
using TodoApp.Application.Todos.Dtos;
using TodoApp.Application.Todos.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Todos;
using TodoApp.Application.Todos.Comments.Queries;
using TodoApp.Application.Todos.Comments.Commands;

namespace TodoApp.Presentation.Controllers;

partial class TodosController
{
    [HttpGet("{todoId}/Comments")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<TodoDto>))]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<CommentDto>> GetComments(string todoId, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetComments(todoId, page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{todoId}/Comments/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CommentDto>> GetCommentById(string todoId, string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCommentById(todoId, id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost("{todoId}/Comments")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CommentDto>> CreateComment(string todoId, CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateComment(todoId, request.Text), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetCommentById), new { todoId = todoId, id = data.Id }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{todoId}/Comments/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteComment(string todoId, string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteComment(todoId, id), cancellationToken);
        return this.HandleResult(result);
    }
}
