namespace TodoApp.Domain.Entities;

public class User : AggregateRoot<string>, IAuditable
{
#nullable disable

    protected User() : base(null)
    {
    }

#nullable restore

    public User(string id, string name, string email)
        : base(id)
    {
        Name = name;
        Email = email;
    }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public User CreatedBy { get; set; } = null!;
    public string CreatedById { get; set; } = null!;
    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }
    public string? LastModifiedById { get; set; }
    public DateTimeOffset? LastModified { get; set; }
}
