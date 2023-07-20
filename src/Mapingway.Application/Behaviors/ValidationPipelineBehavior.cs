using FluentValidation;
using Mapingway.Common.Result;
using Mapingway.Common.Result.Validation;
using MediatR;

namespace Mapingway.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;


    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async Task<TResult> Handle(
        TRequest command, 
        RequestHandlerDelegate<TResult> next, 
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var errors = _validators
            .Select(v => v.ValidateAsync(command, cancellationToken))
            .SelectMany(vr => vr.Result.Errors)
            .Where(vr => vr is not null)
            .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Length != 0)
        {
            return CreateValidationResult(errors);
        }

        return await next();
    }


    private static TResult CreateValidationResult(Error[] errors)
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        var result = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors });

        return (result as TResult)!;
    }
}