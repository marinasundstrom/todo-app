namespace TodoApp.Domain.Events;

public sealed record TodoDescriptionUpdated(int TodoId, string? Description) : DomainEvent;