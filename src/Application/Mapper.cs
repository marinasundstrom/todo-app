using TodoApp.Application.Todos.Dtos;
using TodoApp.Application.Users;

namespace TodoApp.Application;

public static class Mappings
{
    public static TodoDto ToDto(this Todo todo) => new TodoDto(todo.Id, todo.Title, todo.Description, todo.EstimatedHours, todo.RemainingHours, (TodoStatusDto)todo.Status, todo.Created, todo.CreatedBy.ToDto(), todo.LastModified, todo.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new UserDto(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new UserInfoDto();
}
