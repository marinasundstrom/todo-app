namespace TodoApp.Domain.Entities;

public class User : AuditableEntity, IAggregateRoot
{
#nullable disable

    protected User()
    {
    }

#nullable restore

    public User(string id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public string Id { get; private set; }

    public string Name { get; private set; }

    public string Email { get; private set; }
}
