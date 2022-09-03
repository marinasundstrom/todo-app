using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;
using TodoApp.Domain.Events;

namespace TodoApp.Domain.Tests;

public class TodoTest
{
    [Fact]
    public void CreateTodo()
    {
        var todo = new Todo("Foo", "Bar", TodoApp.Domain.Enums.TodoStatus.New);

        Assert.Single(todo.DomainEvents.OfType<TodoCreated>());
    }

    [Fact]
    public void UpdateTitle()
    {
        // Arrange
        var todo = new Todo("Foo", "Bar", TodoApp.Domain.Enums.TodoStatus.New);

        var newTitle = "Zack";

        // Act
        todo.UpdateTitle(newTitle);

        // Assert
        Assert.Equal(newTitle, todo.Title);

        Assert.Single(todo.DomainEvents.OfType<TodoTitleUpdated>());
        Assert.Single(todo.DomainEvents.OfType<TodoUpdated>());
    }

    [Fact]
    public void UpdateDescription()
    {
        // Arrange
        var todo = new Todo("Foo", "Bar", TodoApp.Domain.Enums.TodoStatus.New);

        var newDescription = "This is a new description";

        // Act
        todo.UpdateDescription(newDescription);

        // Assert
        Assert.Equal(newDescription, todo.Description);

        Assert.Single(todo.DomainEvents.OfType<TodoDescriptionUpdated>());
        Assert.Single(todo.DomainEvents.OfType<TodoUpdated>());
    }

    [Fact]
    public void UpdateStatus()
    {
        // Arrange
        var todo = new Todo("Foo", "Bar", TodoApp.Domain.Enums.TodoStatus.New);

        var newStatus = TodoStatus.Completed;

        // Act
        todo.UpdateStatus(newStatus);

        // Assert
        Assert.Equal(TodoStatus.Completed, todo.Status);

        Assert.Single(todo.DomainEvents.OfType<TodoStatusUpdated>());
        Assert.Single(todo.DomainEvents.OfType<TodoUpdated>());
    }
}
