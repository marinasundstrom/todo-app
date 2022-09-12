namespace TodoApp.Domain.Events;

public class TodoCreated : DomainEvent
{
    public TodoCreated(int todoId)
    {
        TodoId = todoId;
    }

    public int TodoId { get; }
}
