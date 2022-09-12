﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistance;

public class Seed
{
    public static async Task SeedData(DbContext context)
    {
        await context.SaveChangesAsync();
    }
}