using FluentValidation;
using FluentValidation.Results;
using Mapingway.Application.Behaviors.Validation;
using Mapingway.SharedKernel.Result;
using MediatR;
using ValidationResult = Mapingway.Application.Behaviors.Validation.ValidationResult;

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

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken ct)
    {
        if (!_validators.Any()) return await next();

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(request, ct)));

        var failures = validationResults
            .Where(vr => !vr.IsValid)
            .SelectMany(vr => vr.Errors)
            .Distinct()
            .ToList();

        if (failures.Count == 0) return await next();

         return ValidationFailedResult(failures);
    }

    private static TResult ValidationFailedResult(IEnumerable<ValidationFailure> failures)
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithFailures(failures) as TResult)!;
        }

        //If generic version of Result<TValue> used
        var resultGenericParameter = typeof(TResult).GenericTypeArguments[0];

        var result = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(resultGenericParameter)
            .GetMethod(nameof(ValidationResult.WithFailures))!
            .Invoke(null, [failures]);

        return (result as TResult)!;
    }
}
