using TodoApp.Domain.Events;

namespace TodoApp.Domain.Entities;

public class Comment : AuditableEntity, IAggregateRoot<string>
{
    protected Comment()
    {
    }

    public Comment(string text)
    {
        Id = Guid.NewGuid().ToString();
        Text = text;

        AddDomainEvent(new TodoCreated(Id));
    }

    public string Id { get; private set; } = null!;
    
    public string TodoId { get; private set; } = null!;

    public string Text { get; private set; } = null!;
}