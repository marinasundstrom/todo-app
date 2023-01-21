using TodoApp.Application.Entities;
using TodoApp.Application.Enums;
using TodoApp.Application.Events;

namespace TodoApp.Application.Tests;

public class TodoTest
{
    [Fact]
    public void CreateTodo()
    {
        var todo = new Todo("Foo", "Bar", TodoApp.Application.Enums.TodoStatus.NotStarted);

        //todo.DomainEvents.OfType<TodoCreated>().Should().ContainSingle();
    }

    [Fact]
    public void UpdateTitle()
    {
        // Arrange
        var oldTitle = "Foo";

        var todo = new Todo(oldTitle, "Bar", TodoApp.Application.Enums.TodoStatus.NotStarted);

        var newTitle = "Zack";

        // Act
        todo.UpdateTitle(newTitle);

        // Assert
        todo.Title.Should().NotBe(oldTitle);
        todo.Title.Should().Be(newTitle);

        todo.DomainEvents.OfType<TodoTitleUpdated>().Should().ContainSingle();
        todo.DomainEvents.OfType<TodoUpdated>().Should().ContainSingle();
    }

    [Fact]
    public void UpdateDescription()
    {
        // Arrange
        var oldDescription = "Bar";

        var todo = new Todo("Foo", oldDescription, TodoApp.Application.Enums.TodoStatus.NotStarted);

        var newDescription = "This is a new description";

        // Act
        todo.UpdateDescription(newDescription);

        // Assert
        todo.Description.Should().NotBe(oldDescription);
        todo.Description.Should().Be(newDescription);

        todo.DomainEvents.OfType<TodoDescriptionUpdated>().Should().ContainSingle();
        todo.DomainEvents.OfType<TodoUpdated>().Should().ContainSingle();
    }

    [Fact]
    public void UpdateStatus()
    {
        // Arrange
        var oldStatus = TodoApp.Application.Enums.TodoStatus.NotStarted;

        var todo = new Todo("Foo", "Bar", oldStatus);

        var newStatus = TodoStatus.Completed;

        // Act
        todo.UpdateStatus(newStatus);

        // Assert
        todo.Status.Should().NotBe(oldStatus);
        todo.Status.Should().Be(TodoStatus.Completed);

        todo.DomainEvents.OfType<TodoStatusUpdated>().Should().ContainSingle();
        todo.DomainEvents.OfType<TodoUpdated>().Should().ContainSingle();
    }
}
