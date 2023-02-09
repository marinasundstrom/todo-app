using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TodoApp.Features.Todos;

[Authorize]
public sealed class TodosHub : Hub<ITodosHubClient>
{
    public override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext is not null)
        {
            if (httpContext.Request.Query.TryGetValue("itemId", out var itemId))
            {
                Groups.AddToGroupAsync(this.Context.ConnectionId, $"item-{itemId}");
            }
        }

        return base.OnConnectedAsync();
    }
}

public interface ITodosHubClient
{
    Task Created(int todoId, string title);

    Task Updated(int todoId, string title);

    Task Deleted(int todoId, string title);

    Task TitleUpdated(int todoId, string title);

    Task DescriptionUpdated(int todoId, string? description);

    Task StatusUpdated(int todoId, TodoStatusDto status);

    Task EstimatedHoursUpdated(int todoId, double? hours);

    Task RemainingHoursUpdated(int todoId, double? hours);
}