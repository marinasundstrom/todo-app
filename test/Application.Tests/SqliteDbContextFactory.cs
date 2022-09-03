using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

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
