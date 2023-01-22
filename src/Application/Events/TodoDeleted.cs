namespace TodoApp.Application.Events;

public sealed record TodoDeleted(int TodoId, string Title) : DomainEvent;