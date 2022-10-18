using FluentValidation;
using MediatR;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.Commands;

public sealed record UpdateAssignedUser(int Id, string? UserId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateAssignedUser>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateAssignedUser, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAssignedUser request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            if (request.UserId is not null)
            {
                var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

                if (user is null)
                {
                    return Result.Failure(Errors.Users.UserNotFound);
                }
            }

            todo.UpdateAssignedTo(request.UserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
