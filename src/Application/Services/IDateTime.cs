namespace TodoApp.Application.Services;

public interface IDateTime
{
    DateTimeOffset Now { get; }
}

