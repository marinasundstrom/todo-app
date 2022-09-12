using MediatR;
using FluentValidation;

namespace TodoApp.Application.Todos.Commands;

public record UpdateDescription(int Id, string? Description) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateDescription>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public class Handler : IRequestHandler<UpdateDescription, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result> Handle(UpdateDescription request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if(todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            if(todo.UpdateDescription(request.Description))
            {
                await todoRepository.SaveChangesAsync(cancellationToken);

                await todoNotificationService.Updated(todo.Id);
                await todoNotificationService.DescriptionUpdated(todo.Id, todo.Description);
            }
            
            return Result.Success();
        }
    }
}
