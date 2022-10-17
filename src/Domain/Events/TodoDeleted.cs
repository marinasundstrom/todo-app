namespace TodoApp.Domain.Events;

public sealed class TodoDeleted : DomainEvent
{
    public TodoDeleted(int todoId, string title)
    {
        TodoId = todoId;
        Title = title;
    }

    public int TodoId { get; }

    public string Title { get; }
}
