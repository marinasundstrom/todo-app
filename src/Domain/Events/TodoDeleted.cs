namespace TodoApp.Domain.Events;

public class TodoDeleted : DomainEvent
{
    public TodoDeleted(int todoId)
    {
        TodoId = todoId;
    }

    public int TodoId { get; }
}
