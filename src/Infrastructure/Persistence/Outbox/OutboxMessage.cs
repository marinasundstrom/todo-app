using System;

namespace TodoApp.Infrastructure.Persistence.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }

    public DateTime OccurredOnUtc { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string Type { get; set; } = null!;

    public string Content { get; set; } = null!;
}

