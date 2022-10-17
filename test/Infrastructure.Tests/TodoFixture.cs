using System.Data.Common;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoApp.Infrastructure;

public class TodoFixture : IDisposable
{
    private readonly IDomainEventDispatcher fakeDomainEventDispatcher;
    private readonly ICurrentUserService fakeCurrentUserService;
    private readonly IDateTime fakeDateTimeService;
    private SqliteConnection connection = null!;

    public TodoFixture()
    {
        fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        fakeCurrentUserService = Substitute.For<ICurrentUserService>();
        fakeDateTimeService = Substitute.For<IDateTime>();
    }

    public ApplicationDbContext CreateDbContext()
    {
        string dbName = $"testdb";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
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