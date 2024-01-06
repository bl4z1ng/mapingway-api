using FluentValidation;
using Mapingway.SharedKernel.Result;
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

        throw new ValidationException(failures);

        //return ValidationFailedResult(failures);
    }

    // Context: tried multiple versions of it to not throw exceptions, didn't find the solution not using reflection
    // or dynamic type usage, so using exceptions as a temporary solution
    // private static TResult ValidationFailedResult(IEnumerable<ValidationFailure> failures)
    // {
    //     var validationError = ValidationError.WithFailures(failures);
    //     if (typeof(TResult) == typeof(Result))
    //     {
    //         return (Result.Failure(validationError) as TResult)!;
    //     }
    //
    //     //If generic version of Result<TValue> used
    //     var resultGenericParameter = typeof(TResult).GenericTypeArguments[0];
    //
    //     var failure = typeof(Result)
    //         .GetMethods()
    //         .First(info => info is { Name: nameof(Result.Failure), IsGenericMethod: true } )
    //         .MakeGenericMethod(resultGenericParameter);
    //
    //     var result = failure.Invoke(null, [validationError]);
    //
    //     return (result as TResult)!;
    // }
}
