using TodoApp.Domain.Enums;
using MediatR;

namespace TodoApp.Domain.Events;

public class TodoStatusUpdated : DomainEvent, INotification
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
