namespace TodoApp.Domain.Events;

public class TodoUpdated : DomainEvent
{
    public TodoUpdated(string todoId)
    {
        TodoId = todoId;
    }

    public string TodoId { get; }
}
