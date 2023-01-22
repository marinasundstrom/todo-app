namespace TodoApp.Application.Events;

public sealed record TodoDescriptionUpdated(int TodoId, string? Description) : DomainEvent;