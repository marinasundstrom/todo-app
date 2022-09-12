﻿using System.Threading;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Specifications;

namespace TodoApp.Infrastructure.Persistance.Repositories;

public class TodoRepository : RepositoryBase<Todo, int>, ITodoRepository
{
    public TodoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override IQueryable<Todo> GetAll()
    {
        return dbSet.AsQueryable();
    }

    public override async Task<Todo?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
}
