using MediatR;
using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Events;

public sealed class TodoStatusUpdated : DomainEvent, INotification
{
    public TodoStatusUpdated(int todoId, TodoStatus newStatus, TodoStatus oldStatus)
    {
        TodoId = todoId;
        NewStatus = newStatus;
        OldStatus = oldStatus;
    }

    public int TodoId { get; }
    public TodoStatus NewStatus { get; }
    public TodoStatus OldStatus { get; }
}
