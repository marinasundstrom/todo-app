using Microsoft.AspNetCore.SignalR;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Presentation.Hubs;

public class TodoNotificationService : ITodoNotificationService
{
    private readonly IHubContext<TodosHub, ITodosHubClient> hubsContext;

    public TodoNotificationService(IHubContext<TodosHub, ITodosHubClient> hubsContext)
    {
        this.hubsContext = hubsContext;
    }

    public async Task Created(int todoId, string title)
    {
        await hubsContext.Clients.All.Created(todoId, title);
    }

    public async Task Updated(int todoId, string title)
    {
        await hubsContext.Clients.All.Updated(todoId, title);
    }

    public async Task Deleted(int todoId, string title)
    {
        await hubsContext.Clients.All.Deleted(todoId, title);
    }

    public async Task DescriptionUpdated(int todoId, string? description)
    {
        await hubsContext.Clients.All.DescriptionUpdated(todoId, description);
    }

    public async Task StatusUpdated(int todoId, TodoStatusDto status)
    {
        await hubsContext.Clients.All.StatusUpdated(todoId, status);
    }

    public async Task TitleUpdated(int todoId, string title)
    {
        await hubsContext.Clients.All.TitleUpdated(todoId, title);
    }

    public async Task EstimatedHoursUpdated(int todoId, double? hours)
    {
        await hubsContext.Clients.All.EstimatedHoursUpdated(todoId, hours);
    }

    public async Task RemainingHoursUpdated(int todoId, double? hours)
    {
        await hubsContext.Clients.All.RemainingHoursUpdated(todoId, hours);
    }
}