namespace TodoApp.Domain.Events;

public sealed record TodoUpdated(int TodoId) : DomainEvent;