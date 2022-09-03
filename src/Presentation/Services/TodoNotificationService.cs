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

    public async Task Created(string todoId)
    {
        await hubsContext.Clients.All.Created(todoId);
    }

    public async Task Updated(string todoId)
    {
        await hubsContext.Clients.All.Updated(todoId);
    }

    public async Task Deleted(string todoId)
    {
        await hubsContext.Clients.All.Deleted(todoId);
    }

    public async Task DescriptionUpdated(string todoId, string? description)
    {
        await hubsContext.Clients.Group($"item-{todoId}").DescriptionUpdated(todoId, description);
    }

    public async Task StatusUpdated(string todoId, TodoStatusDto status)
    {
        await hubsContext.Clients.Group($"item-{todoId}").StatusUpdated(todoId, status);
    }

    public async Task TitleUpdated(string todoId, string title)
    {
        await hubsContext.Clients.All.TitleUpdated(todoId, title);
    }
}