using TodoApp.Domain.Enums;
using MediatR;

namespace TodoApp.Domain.Events;

public class TodoStatusUpdated : DomainEvent, INotification
{
    public TodoStatusUpdated(string todoId, TodoStatus newStatus, TodoStatus oldStatus)
    {
        TodoId = todoId;
        NewStatus = newStatus;
        OldStatus = oldStatus;
    }

    public string TodoId { get; }
    public TodoStatus NewStatus { get; }
    public TodoStatus OldStatus { get; }
}
