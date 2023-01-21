using System;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Specifications;

public class TodosWithStatus : BaseSpecification<Todo>
{
    public TodosWithStatus(TodoStatus status)
    {
        Criteria = todo => todo.Status == status;
    }
}

