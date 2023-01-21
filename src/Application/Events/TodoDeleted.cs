namespace TodoApp.Domain.Events;

public sealed record TodoDeleted(int TodoId, string Title) : DomainEvent;