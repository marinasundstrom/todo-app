using System;
using TodoApp.Application.Entities;
using TodoApp.Application.Enums;

namespace TodoApp.Application.Specifications;

public class TodosWithStatus : BaseSpecification<Todo>
{
    public TodosWithStatus(TodoStatus status)
    {
        Criteria = todo => todo.Status == status;
    }
}

