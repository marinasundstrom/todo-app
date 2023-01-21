namespace TodoApp.Domain.Events;

public sealed record TodoEstimatedHoursUpdated(int TodoId, double? Hours, double? OldHours) : DomainEvent;