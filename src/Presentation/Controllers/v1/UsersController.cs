using MassTransit;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TodoApp.Application;
using TodoApp.Application.Common;
using TodoApp.Application.Todos.Dtos;
using TodoApp.Application.Todos.Queries;
using TodoApp.Application.Users;

namespace TodoApp.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<UserDto>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<UserDto>> GetUsers(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetUsers(page, pageSize, searchTerm, sortBy, sortDirection), cancellationToken);

    [HttpGet("UserInfo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfoDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserInfo(), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfoDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<UserInfoDto>> CreateUser(CreateUserDto request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateUser(request.Name, request.Email), cancellationToken);
        return this.HandleResult(result);
    }
}

public sealed record CreateUserDto(string Name, string Email);