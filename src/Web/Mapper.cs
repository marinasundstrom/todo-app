using TodoApp.Features.Todos;
using TodoApp.Features.Users;

namespace TodoApp;

public static class Mappings
{
    public static TodoDto ToDto(this Todo todo) => new TodoDto(todo.Id, todo.Title, todo.Description, (TodoStatusDto)todo.Status, todo.AssignedTo?.ToDto(), todo.EstimatedHours, todo.RemainingHours, todo.Created, todo.CreatedBy.ToDto(), todo.LastModified, todo.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new UserDto(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new UserInfoDto(user.Id, user.Name);
}
