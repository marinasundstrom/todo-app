﻿using System.Threading;
using TodoApp.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Persistance.Repositories;

public class TodoRepository : RepositoryBase<Todo, string>, ITodoRepository
{
    public TodoRepository(ApplicationContext context) : base(context)
    {
    }

    public override IQueryable<Todo> GetAll()
    {
        return dbSet.AsQueryable();
    }

    public override async Task<Todo?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
}
