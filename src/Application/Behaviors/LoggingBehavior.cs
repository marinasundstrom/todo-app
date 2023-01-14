using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using ValidationException = TodoApp.Application.Exceptions.ValidationException;

namespace TodoApp.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    //where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        logger.LogInformation(
            "Starting request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        var response = await next(request, cancellationToken);

        if (response is Result result && result.IsFailure)
        {
            logger.LogError(
            "Request failure {@RequestName}, {@Error} {@DateTimeUtc}",
            typeof(TRequest).Name,
            result.GetError(),
            DateTime.UtcNow);
        }

        logger.LogInformation(
                    "Completed request {@RequestName}, {@DateTimeUtc}",
                    typeof(TRequest).Name,
                    DateTime.UtcNow);

        return response;
    }

    ValueTask<TResponse> IPipelineBehavior<TRequest, TResponse>.Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        throw new NotImplementedException();
    }
}