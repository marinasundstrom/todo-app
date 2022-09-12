using MediatR;

namespace TodoApp.Domain.Events;

public class TodoRemainingHoursUpdated : DomainEvent, INotification
{
    public TodoRemainingHoursUpdated(int todoId, double? hours, double? oldHourse)
    {
        TodoId = todoId;
        Hours = hours;
        OldHourse = oldHourse;
    }

    public int TodoId { get; }

    public double? Hours { get; }
    public double? OldHourse { get; }
    public double? OldHours { get; }
}
