using MediatR;

namespace TodoApp.Domain.Events;

public sealed class TodoDescriptionUpdated : DomainEvent
{
    public TodoDescriptionUpdated(int todoId, string? description)
    {
        TodoId = todoId;
        Description = description;
    }

    public int TodoId { get; }

    public string? Description { get; }
}