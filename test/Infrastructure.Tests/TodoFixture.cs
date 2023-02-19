using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoApp.Services;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Persistence.Interceptors;

namespace TodoApp.Infrastructure;

public class TodoFixture : IDisposable
{
    private readonly ICurrentUserService fakeCurrentUserService;
    private readonly IDateTime fakeDateTimeService;
    private SqliteConnection connection = null!;

    public TodoFixture()
    {
        fakeCurrentUserService = Substitute.For<ICurrentUserService>();
        fakeCurrentUserService.UserId.Returns("foo");

        fakeDateTimeService = Substitute.For<IDateTime>();
        fakeDateTimeService.Now.Returns(DateTime.UtcNow);
    }

    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .AddInterceptors(new AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService), new OutboxSaveChangesInterceptor())
           .UseSqlite(GetDbConnection())
           .Options;

        var context = new ApplicationDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }

    private DbConnection GetDbConnection()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        return connection;
    }

    public void Dispose()
    {
        connection.Close();
    }
}