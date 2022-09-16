namespace TodoApp.Domain.Events;

public sealed class TodoUpdated : DomainEvent
{
    public TodoUpdated(int todoId)
    {
        TodoId = todoId;
    }

    public int TodoId { get; }
}
