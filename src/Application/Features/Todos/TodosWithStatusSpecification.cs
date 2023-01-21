using TodoApp.Application.Specifications;

namespace TodoApp.Application.Features.Todos;

public class TodosWithStatusSpecification : BaseSpecification<Todo>
{
    public TodosWithStatusSpecification(TodoStatus status)
    {
        Criteria = todo => todo.Status == status;
    }
}

