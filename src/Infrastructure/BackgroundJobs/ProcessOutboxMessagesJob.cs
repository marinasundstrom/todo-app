using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Persistence.Outbox;

namespace TodoApp.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext dbContext;
    private readonly IDomainEventDispatcher domainEventDispatcher;

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
    {
        this.dbContext = dbContext;
        this.domainEventDispatcher = domainEventDispatcher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (OutboxMessage outboxMessage in messages)
        {
            DomainEvent? domainEvent = JsonConvert
                .DeserializeObject<DomainEvent>(outboxMessage.Content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            if (domainEvent is null)
            {
                continue;
            }

            await domainEventDispatcher.Dispatch(domainEvent, context.CancellationToken);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}

