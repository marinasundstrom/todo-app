using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Presentation.Hubs;

public interface ITodosHubClient
{
    Task Created(string todoId);

    Task Updated(string todoId);

    Task Deleted(string todoId);

    Task TitleUpdated(string todoId, string title);

    Task DescriptionUpdated(string todoId, string? description);

    Task StatusUpdated(string todoId, TodoStatusDto status);
}