using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common;
using TodoApp.Application.Extensions;
using TodoApp.Application.Features.Todos.Commands;
using TodoApp.Application.Features.Todos.Queries;

namespace TodoApp.Application.Features.Todos;

public static class Endpoints
{
    public static WebApplication MapTodoEndpoints(this WebApplication app)
    {
        var todos = app.NewVersionedApi("Todos")
           .HasApiVersion(1, 0);

        var group = todos.MapGroup("/v{version:apiVersion}/Todos")
            .WithTags("Todos")
            .RequireAuthorization()
            .WithOpenApi();

        group.MapGet("/", GetTodos)
            .Produces<ItemsResult<TodoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting("fixed");

        group.MapGet("/{id}", GetTodoById)
            .Produces<TodoDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName(nameof(GetTodoById));

        group.MapPost("/", CreateTodo)
            .Produces<TodoDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{id}", DeleteTodo)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id}/Title", UpdateTitle)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id}/Description", UpdateDescription)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id}/Status", UpdateStatus)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id}/AssignedUser", UpdateAssignedUser)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id}/EstimatedHours", UpdateEstimatedHours)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id}/RemainingHours", UpdateRemainingHours)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    public static async Task<ItemsResult<TodoDto>> GetTodos(TodoStatusDto? status, string? assignedTo, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default, IMediator mediator = default!)
        => await mediator.Send(new GetTodos(status, assignedTo, page, pageSize, sortBy, sortDirection), cancellationToken);

    public static async Task<IResult> GetTodoById(int id, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new GetTodoById(id), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> CreateTodo(CreateTodoRequest request, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new CreateTodo(request.Title, request.Description, request.Status, request.AssignedTo, request.EstimatedHours, request.RemainingHours), cancellationToken);
        return result.Handle(
            onSuccess: data => Results.CreatedAtRoute(nameof(GetTodoById), new { id = data.Id }, data),
            onError: error => Results.Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    public static async Task<IResult> DeleteTodo(int id, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new DeleteTodo(id), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> UpdateTitle(int id, [FromBody] string title, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new UpdateTitle(id, title), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> UpdateDescription(int id, [FromBody] string? description, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new UpdateDescription(id, description), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> UpdateStatus(int id, [FromBody] TodoStatusDto status, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new UpdateStatus(id, status), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> UpdateAssignedUser(int id, [FromBody] string? userId, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new UpdateAssignedUser(id, userId), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> UpdateEstimatedHours(int id, [FromBody] double? hours, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new UpdateEstimatedHours(id, hours), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> UpdateRemainingHours(int id, [FromBody] double? hours, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new UpdateRemainingHours(id, hours), cancellationToken);
        return HandleResult(result);
    }

    private static IResult HandleResult(Result result) => result.Handle(
            onSuccess: () => Results.Ok(),
            onError: error =>
            {
                if (error.Id.EndsWith("NotFound"))
                {
                    return Results.NotFound();
                }
                return Results.Problem(detail: error.Detail, title: error.Title, type: error.Id);
            });

    private static IResult HandleResult<T>(Result<T> result) => result.Handle(
            onSuccess: data => Results.Ok(data),
            onError: error =>
            {
                if (error.Id.EndsWith("NotFound"))
                {
                    return Results.NotFound();
                }
                return Results.Problem(detail: error.Detail, title: error.Title, type: error.Id);
            });
}

public sealed record CreateTodoRequest(string Title, string? Description, TodoStatusDto Status, string? AssignedTo, double? EstimatedHours, double? RemainingHours);
