namespace TodoApp.Application.Common;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);