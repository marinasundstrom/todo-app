using TodoApp.Domain.Enums;
using TodoApp.Domain.Events;

namespace TodoApp.Domain.Entities;

public class Todo : AuditableEntity, IAggregateRoot<string>
{
    protected Todo()
    {
    }

    public Todo(string title, string? description, TodoStatus status = TodoStatus.New)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        Status = status;

        AddDomainEvent(new TodoCreated(Id));
    }

    public string Id { get; private set; } = null!;

    public string Title { get; private set; } = null!;

    public bool UpdateTitle(string title) 
    {
        var oldTitle = Title;
        if(title != oldTitle) 
        {
            Title = title;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoTitleUpdated(Id, title));

            return true;
        }

        return false;
    }

    public string? Description { get; private set; }

    public bool UpdateDescription(string? description) 
    {
        var oldDescription = Description;
        if(description != oldDescription) 
        {
            Description = description;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoDescriptionUpdated(Id, description));

            return true;
        }

        return false;
    }

    public TodoStatus Status  { get; private set; }

    public bool UpdateStatus(TodoStatus status) 
    {
        var oldStatus = Status;
        if(status != oldStatus) 
        {
            Status = status;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoStatusUpdated(Id, status, oldStatus));
            
            return true;
        }

        return false;
    }
}
