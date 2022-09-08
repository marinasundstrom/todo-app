using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistance.Interceptors;

namespace TodoApp.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        private IDomainEventDispatcher _domainEventDispatcher;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
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

            ApplySoftDeleteQueryFilter(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            // INFO: This code adds a query filter to any object deriving from Entity
            //       and that is implementing the ISoftDelete interface.
            //       The generated expressions correspond to: (e) => e.Deleted == null.
            //       Causing the entity not to be included in the result if Deleted is not null.
            //       There are other better ways to approach non-destructive "deletion".

            var entityBaseType = typeof(Entity);
            var softDeleteInterface = typeof(ISoftDelete);
            var deletedProperty = softDeleteInterface.GetProperty(nameof(ISoftDelete.Deleted));

            foreach (var entityType in softDeleteInterface.Assembly
                .GetTypes()
                .Where(candidateEntityType => entityBaseType.IsAssignableFrom(candidateEntityType))
                .Where(candidateEntityType => softDeleteInterface.IsAssignableFrom(candidateEntityType)))
            {
                var param = Expression.Parameter(entityType, "entity");
                var body = Expression.Equal(Expression.Property(param, deletedProperty!), Expression.Constant(null));
                var lambda = Expression.Lambda(body, param);

                modelBuilder.Entity(entityType).HasQueryFilter(lambda);
            }
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
                .Entries<Entity>()
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
