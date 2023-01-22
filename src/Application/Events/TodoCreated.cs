namespace TodoApp.Application.Events;

public sealed record TodoCreated(int TodoId) : DomainEvent;
