using FluentValidation;
using MediatR;

namespace TodoApp.Application.Users;

public record CreateUser(string Name, string Email) : IRequest<Result<UserInfoDto>>
{
    public class Validator : AbstractValidator<CreateUser>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Name).NotEmpty().EmailAddress();
        }
    }

    public class Handler : IRequestHandler<CreateUser, Result<UserInfoDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<UserInfoDto>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            userRepository.Add(new User(currentUserService.UserId!, request.Name, request.Email));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success<UserInfoDto>(new UserInfoDto());
        }
    }
}
