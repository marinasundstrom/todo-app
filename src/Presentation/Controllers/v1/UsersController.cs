using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        var result = await mediator.Send(new CreateUser(request.Name), cancellationToken);
        return this.HandleResult(result);
    }
}

public sealed record CreateUserDto(string Name, string Email);