using MediatR;

namespace TodoApp.Domain.Events;

public class TodoTitleUpdated : DomainEvent, INotification
{
    public TodoTitleUpdated(string todoId, string title)
    {
        TodoId = todoId;
        Title = title;
    }

    public string TodoId { get; }

    public string Title { get; }
}
