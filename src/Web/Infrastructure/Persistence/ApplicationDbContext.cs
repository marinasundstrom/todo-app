using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApp.Domain;
using TodoApp.Domain.ValueObjects;
using TodoApp.Infrastructure.Persistence.ValueConverters;

namespace TodoApp.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplySoftDeleteQueryFilter(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                            || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
    }

    private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
    {
        // INFO: This code adds a query filter to any object deriving from Entity
        //       and that is implementing the ISoftDelete interface.
        //       The generated expressions correspond to: (e) => e.Deleted == null.
        //       Causing the entity not to be included in the result if Deleted is not null.
        //       There are other better ways to approach non-destructive "deletion".

        var softDeleteInterface = typeof(ISoftDelete);
        var deletedProperty = softDeleteInterface.GetProperty(nameof(ISoftDelete.Deleted));

        foreach (var entityType in softDeleteInterface.Assembly
            .GetTypes()
            .Where(candidateEntityType => candidateEntityType != typeof(ISoftDelete))
            .Where(candidateEntityType => softDeleteInterface.IsAssignableFrom(candidateEntityType)))
        {
            var param = Expression.Parameter(entityType, "entity");
            var body = Expression.Equal(Expression.Property(param, deletedProperty!), Expression.Constant(null));
            var lambda = Expression.Lambda(body, param);

            modelBuilder.Entity(entityType).HasQueryFilter(lambda);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<TodoId>().HaveConversion<TodoIdConverter>();
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();
    }

#nullable disable

    public DbSet<Todo> Todos { get; set; }

    public DbSet<User> Users { get; set; }

#nullable restore
}
