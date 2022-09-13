using TodoApp.Infrastructure.Services;

namespace TodoApp.Infrastructure.Persistance.Repositories.Mocks;

public sealed class MockUnitOfWork : IUnitOfWork
{
    private static List<IAggregateRoot> items = new List<IAggregateRoot>();
    private readonly IDomainEventDispatcher domainEventDispatcher;
    private List<IAggregateRoot> newItems = new List<IAggregateRoot>();

    public MockUnitOfWork(IDomainEventDispatcher domainEventDispatcher)
    {
        this.domainEventDispatcher = domainEventDispatcher;
    }

    public List<IAggregateRoot> Items => items;

    public List<IAggregateRoot> NewItems => newItems;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DomainEvent[] events = GetDomainEvents();

        // Simulating save to database
        newItems.Clear();

        await DispatchEvents(events, cancellationToken);

        return 0;
    }

    public async Task DispatchEvents(DomainEvent[] events, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await domainEventDispatcher.Dispatch(@event, cancellationToken);
        }
    }

    private DomainEvent[] GetDomainEvents()
    {
        return newItems
            .OfType<IHasDomainEvents>()
            .Select(x => x.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();
    }
}