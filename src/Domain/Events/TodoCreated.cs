namespace TodoApp.Domain.Events;

public sealed class TodoCreated : DomainEvent
{
    public TodoCreated(int todoId)
    {
        TodoId = todoId;
    }

    public int TodoId { get; }
}
