using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Common;

namespace TodoApp.Application.Features.Users;

public static class Endpoints
{
    public static WebApplication AddUsersEndpoints(this WebApplication app)
    {
        var users = app.NewVersionedApi("Users");

        var group = users.MapGroup("/v{version:apiVersion}/Users")
            .WithTags("Users")
            .RequireAuthorization()
            .WithOpenApi()
            .HasApiVersion(1, 0);

        group.MapGet("/", GetUsers)
            .Produces<ItemsResult<UserDto>>(StatusCodes.Status200OK);

        group.MapGet("/UserInfo", GetUserInfo)
            .Produces<UserInfoDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName(nameof(GetUserInfo));

        group.MapPost("/", CreateUser)
            .Produces<UserInfoDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        var group2 = users.MapGroup("/v{version:apiVersion}/Users")
           .WithTags("Users")
           .RequireAuthorization()
           .WithOpenApi()
           .HasApiVersion(2, 0);

        group2.MapGet("/", GetUsers)
            .Produces<ItemsResult<UserDto>>(StatusCodes.Status200OK);

        return app;
    }
    public static async Task<ItemsResult<UserDto>> GetUsers(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default, IMediator mediator = default!)
        => await mediator.Send(new GetUsers(page, pageSize, searchTerm, sortBy, sortDirection), cancellationToken);

    public static async Task<IResult> GetUserInfo(CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new GetUserInfo(), cancellationToken);
        return HandleResult(result);
    }

    public static async Task<IResult> CreateUser(CreateUserDto request, CancellationToken cancellationToken, IMediator mediator)
    {
        var result = await mediator.Send(new CreateUser(request.Name, request.Email), cancellationToken);
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

public sealed record CreateUserDto(string Name, string Email);