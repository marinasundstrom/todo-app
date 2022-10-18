namespace TodoApp.Domain.Entities;

public abstract class AuditableEntity : Entity
{
    public User CreatedBy { get; set; } = null!;
    public string CreatedById { get; set; } = null!;
    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }
    public string? LastModifiedById { get; set; }
    public DateTimeOffset? LastModified { get; set; }
}