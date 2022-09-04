using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Persistance;

namespace Tests;

public static class SqliteDbContextFactory
{
    public static ApplicationDbContext CreateDbContext(string name, IDomainEventDispatcher fakeDomainEventDispatcher, ICurrentUserService fakeCurrentUserService, IDateTime fakeDateTimeService)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseSqlite($"Data Source={name}.db")
           .Options;

        var context = new ApplicationDbContext(options, fakeDomainEventDispatcher,
            new TodoApp.Infrastructure.Persistance.Interceptors.AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService));

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}
