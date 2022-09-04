using TodoApp.Infrastructure.Persistance;

namespace TodoApp.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}