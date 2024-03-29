﻿using TodoApp.Domain.Specifications;

namespace TodoApp.Features.Todos;

public class TodosWithStatusSpecification : BaseSpecification<Todo>
{
    public TodosWithStatusSpecification(TodoStatus status)
    {
        Criteria = todo => todo.Status == status;
    }
}

