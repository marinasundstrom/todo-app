using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Services;

public interface ITodoNotificationService
{
    Task Created(string todoId);

    Task Updated(string todoId);

    Task Deleted(string todoId);
    
    Task TitleUpdated(string todoId, string title);

    Task DescriptionUpdated(string todoId, string? description);

    Task StatusUpdated(string todoId, TodoStatusDto status);
}
