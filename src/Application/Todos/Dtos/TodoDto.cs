namespace TodoApp.Application.Todos.Dtos;

public record TodoDto(string Id, string Title, string? Description, TodoStatusDto Status, DateTime Created, DateTime? LastModified);

public record CommentDto(string Id, string Text, DateTime Created, DateTime? LastModified);
