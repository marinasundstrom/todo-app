using MediatR;
using TodoApp.Application.Todos.Dtos;
using FluentValidation;

namespace TodoApp.Application.Todos.Commands;

public record UpdateStatus(string Id, TodoStatusDto Status) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateStatus>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<UpdateStatus, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if(todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            if(todo.UpdateStatus((Domain.Enums.TodoStatus)request.Status)) 
            {
                await todoRepository.SaveChangesAsync(cancellationToken);

                await todoNotificationService.Updated(todo.Id);
                await todoNotificationService.StatusUpdated(todo.Id, (TodoStatusDto)todo.Status);
            }       

            return Result.Success();
        }
    }
}
