namespace TodoApp.Application.Events;

public sealed record TodoTitleUpdated(int TodoId, string Title) : DomainEvent;