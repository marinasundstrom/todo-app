using FluentValidation;
using Mediator;
using ValidationException = TodoApp.Application.Exceptions.ValidationException;

namespace TodoApp.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (!_validators.Any())
        {
            return await next(request, cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var errorsDictionary = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);

        if (errorsDictionary.Any())
        {
            throw new ValidationException(errorsDictionary);
        }

        return await next(request, cancellationToken);
    }
}