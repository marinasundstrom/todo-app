namespace TodoApp.Domain.Events;

public sealed record TodoCreated(int TodoId) : DomainEvent;
