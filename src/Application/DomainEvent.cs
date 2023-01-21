using MediatR;

namespace TodoApp.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}