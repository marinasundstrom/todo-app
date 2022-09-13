using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Contracts;

namespace TodoApp.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class TestController : ControllerBase
{
    private readonly IPublishEndpoint publishEndpoint;

    public TestController(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task UpdateStatus(int id, [FromBody] TodoStatus status, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new UpdateStatus(id, status), cancellationToken);
    }
}
