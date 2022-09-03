using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Persistance;

namespace Tests;

public static class SqliteDbContextFactory
{
    public static ApplicationContext CreateDbContext(string name, IDomainEventDispatcher fakeDomainEventDispatcher, ICurrentUserService fakeCurrentUserService, IDateTime fakeDateTimeService)
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
           .UseSqlite($"Data Source={name}.db")
           .Options;

        var context = new ApplicationContext(options, fakeDomainEventDispatcher,
            new TodoApp.Infrastructure.Persistance.Interceptors.AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService));

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}
