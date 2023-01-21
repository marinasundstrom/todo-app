namespace TodoApp.Domain.Events;

public sealed record TodoTitleUpdated(int TodoId, string Title) : DomainEvent;