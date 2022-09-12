using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Presentation.Hubs;

public interface ITodosHubClient
{
    Task Created(int todoId);

    Task Updated(int todoId);

    Task Deleted(int todoId);

    Task TitleUpdated(int todoId, string title);

    Task DescriptionUpdated(int todoId, string? description);

    Task StatusUpdated(int todoId, TodoStatusDto status);

    Task EstimatedHoursUpdated(int todoId, double? hours);

    Task RemainingHoursUpdated(int todoId, double? hours);
}