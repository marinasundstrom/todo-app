using TodoApp.Domain.Specifications;

namespace TodoApp.Infrastructure.Persistance.Repositories.Mocks
{
    public class MockRepositoryBase<T, TKey> : IRepository<T, TKey>, IDisposable
        where T : BaseEntity, IAggregateRoot<TKey>
        where TKey : notnull
    {
        protected static List<T> items = new List<T>();
        protected List<T> newItems = new List<T>();
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public MockRepositoryBase(IDomainEventDispatcher domainEventDispatcher)
        {
            this.domainEventDispatcher = domainEventDispatcher;
        }


        public virtual void Add(T item)
        {
            items.Add(item);
            newItems.Add(item);
        }

        public virtual void Dispose()
        {
            foreach (var item in newItems)
            {
                items.Remove(item);
            }
        }

        public virtual Task<T?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var item = items.FirstOrDefault(x => x.Id.Equals(id));
            return Task.FromResult(item);
        }

        public virtual IQueryable<T> GetAll()
        {
            return items.AsQueryable();
        }

        public virtual IQueryable<T> GetAll(ISpecification<T> specification)
        {
            return items.AsQueryable().Where(specification.Criteria);
        }

        public virtual void Remove(T item)
        {
            items.Remove(item);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DomainEvent[] events = GetDomainEvents();

            // Simulating save to database
            newItems.Clear();

            await DispatchEvents(events);

            return 0;
        }

        protected async Task DispatchEvents(DomainEvent[] events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await domainEventDispatcher.Dispatch(@event);
            }
        }

        private DomainEvent[] GetDomainEvents()
        {
            return newItems
                .Select(x => x.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();
        }
    }
}