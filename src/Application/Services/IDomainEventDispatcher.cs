namespace TodoApp.Application.Services;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent);
}
