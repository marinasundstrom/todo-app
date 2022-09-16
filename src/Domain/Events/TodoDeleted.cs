namespace TodoApp.Domain.Events;

public sealed class TodoDeleted : DomainEvent
{
    public TodoDeleted(int todoId)
    {
        TodoId = todoId;
    }

    public int TodoId { get; }
}
