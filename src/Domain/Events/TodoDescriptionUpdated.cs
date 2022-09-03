using MediatR;

namespace TodoApp.Domain.Events;

public class TodoDescriptionUpdated : DomainEvent, INotification
{
    public TodoDescriptionUpdated(string todoId, string? description)
    {
        TodoId = todoId;
        Description = description;
    }

    public string TodoId { get; }
    
    public string? Description { get; }
}