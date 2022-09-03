namespace TodoApp.Domain.Events;

public class TodoDeleted : DomainEvent
{
    public TodoDeleted(string todoId)
    {
        TodoId = todoId;
    }

    public string TodoId { get; }
}
