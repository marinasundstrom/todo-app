using MediatR;
using FluentValidation;

namespace TodoApp.Application.Todos.Commands;

public record UpdateRemainingHours(int Id, double? Hours) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateRemainingHours>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<UpdateRemainingHours, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result> Handle(UpdateRemainingHours request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            if (todo.UpdateRemainingHours(request.Hours))
            {
                await todoRepository.SaveChangesAsync(cancellationToken);

                await todoNotificationService.Updated(todo.Id);
                await todoNotificationService.RemainingHoursUpdated(todo.Id, todo.RemainingHours);
            }

            return Result.Success();
        }
    }
}
