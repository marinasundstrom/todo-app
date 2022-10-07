using MediatR;

namespace TodoApp.Domain;

public abstract class DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}