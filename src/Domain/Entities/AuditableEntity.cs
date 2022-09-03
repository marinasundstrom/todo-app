﻿namespace TodoApp.Domain.Entities;

public class AuditableEntity : BaseEntity
{
    public string CreatedById { get; set; } = null!;
    public DateTime Created { get; set; }

    public string? LastModifiedById { get; set; }
    public DateTime? LastModified { get; set; }
}