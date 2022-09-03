using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Domain.Entities;

public class BaseEntity : IHasDomainEvents
{
    private readonly HashSet<DomainEvent> domainEvents = new HashSet<DomainEvent>();

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => domainEvents;

    public void AddDomainEvent(DomainEvent domainEvent) => domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(DomainEvent domainEvent) => domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => domainEvents.Clear();
}
