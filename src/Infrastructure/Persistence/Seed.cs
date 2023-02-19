using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(DbContext context)
    {
        await context.SaveChangesAsync();
    }
}