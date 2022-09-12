using FluentValidation;
using MediatR;

namespace TodoApp.Application.Todos.Commands;

public record UpdateTitle(int Id, string Title) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateTitle>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);
        }
    }

    public class Handler : IRequestHandler<UpdateTitle, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result> Handle(UpdateTitle request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            if (todo.UpdateTitle(request.Title))
            {
                await todoRepository.SaveChangesAsync(cancellationToken);

                await todoNotificationService.Updated(todo.Id);
                await todoNotificationService.TitleUpdated(todo.Id, todo.Title);
            }

            return Result.Success();
        }
    }
}
