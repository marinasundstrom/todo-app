namespace TodoApp.Domain.Entities;

public abstract class AuditableEntity : Entity
{
    public string CreatedById { get; set; } = null!;
    public DateTimeOffset Created { get; set; }

    public string? LastModifiedById { get; set; }
    public DateTimeOffset? LastModified { get; set; }
}