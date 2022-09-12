using MediatR;

namespace TodoApp.Domain.Events;

public class TodoTitleUpdated : DomainEvent, INotification
{
    public TodoTitleUpdated(int todoId, string title)
    {
        TodoId = todoId;
        Title = title;
    }

    public int TodoId { get; }

    public string Title { get; }
}
