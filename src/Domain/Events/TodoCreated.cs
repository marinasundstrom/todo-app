namespace TodoApp.Domain.Events;

public class TodoCreated : DomainEvent
{
    public TodoCreated(string todoId)
    {
        TodoId = todoId;
    }

    public string TodoId { get; }
}
