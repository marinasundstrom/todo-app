namespace TodoApp.Domain.Entities;

public interface IAggregateRoot
{

}

public interface IAggregateRoot<TKey> : IAggregateRoot
{
    public TKey Id { get; }
}