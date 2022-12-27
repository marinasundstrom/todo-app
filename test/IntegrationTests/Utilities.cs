using TodoApp.Infrastructure.Persistence;

namespace TodoApp.IntegrationTests;

internal class Utilities
{
    public static async Task InitializeDbForTests(ApplicationDbContext db)
    {
        //db.Users.Add(new Domain.Entities.User("1234", "Test Testsson", "test@email.com"));

        await db.SaveChangesAsync();
    }
}