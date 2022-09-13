using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Persistance;

namespace TodoApp.Infrastructure
{
    public class TodoFixture : IDisposable
    {
        private readonly IDomainEventDispatcher fakeDomainEventDispatcher;
        private readonly ICurrentUserService fakeCurrentUserService;
        private readonly IDateTime fakeDateTimeService;

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
               .UseSqlite($"Data Source={dbName}.db")
               .Options;

            var context = new ApplicationDbContext(options,
                new TodoApp.Infrastructure.Persistance.Interceptors.AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService));

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        public void Dispose()
        {

        }
    }
}