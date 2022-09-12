using MediatR;

namespace TodoApp.Domain.Events;

public class TodoEstimatedHoursUpdated : DomainEvent, INotification
{
    public TodoEstimatedHoursUpdated(int todoId, double? hours, double? oldHours)
    {
        TodoId = todoId;
        Hours = hours;
        OldHours = oldHours;
    }

    public int TodoId { get; }

    public double? Hours { get; }

    public double? OldHours { get; }
}
