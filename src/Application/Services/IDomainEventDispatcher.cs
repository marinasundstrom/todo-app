using TodoApp.Application.Domain;

namespace TodoApp.Application.Services;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
