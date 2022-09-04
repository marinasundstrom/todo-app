using TodoApp.Infrastructure.Persistance.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Persistance
{
    public class ApplicationContext : DbContext
    {
        private IDomainEventDispatcher _domainEventDispatcher;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options,
            IDomainEventDispatcher domainEventDispatcher,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }

#nullable disable

    public DbSet<Todo> Todos { get; set; }

#nullable restore

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchEvents();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task DispatchEvents()
        {
            var entities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity);

            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _domainEventDispatcher.Dispatch(domainEvent);
        }
    }
}
