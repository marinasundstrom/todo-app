namespace TodoApp.Application.Events;

public sealed record TodoUpdated(int TodoId) : DomainEvent;