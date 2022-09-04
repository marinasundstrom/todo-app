using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application;

public static class Mappings
{
    public static TodoDto ToDto(this Todo todo) => new TodoDto(todo.Id, todo.Title, todo.Description, (TodoStatusDto)todo.Status, todo.Created, todo.LastModified); 

    public static CommentDto ToDto(this Comment comment) => new CommentDto(comment.Id, comment.Text, comment.Created, comment.LastModified); 
}
