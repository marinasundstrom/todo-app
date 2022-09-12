using FluentValidation;
using MediatR;

namespace TodoApp.Application.Todos.Commands;

public record UpdateEstimatedHours(int Id, double? Hours) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateEstimatedHours>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<UpdateEstimatedHours, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result> Handle(UpdateEstimatedHours request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            if (todo.UpdateEstimatedHours(request.Hours))
            {
                await todoRepository.SaveChangesAsync(cancellationToken);

                await todoNotificationService.Updated(todo.Id);
                await todoNotificationService.EstimatedHoursUpdated(todo.Id, todo.EstimatedHours);
            }

            return Result.Success();
        }
    }
}
