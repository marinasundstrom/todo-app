namespace TodoApp.Domain.Events;

public sealed record TodoRemainingHoursUpdated(int TodoId, double? hHurs, double? OldHours) : DomainEvent;