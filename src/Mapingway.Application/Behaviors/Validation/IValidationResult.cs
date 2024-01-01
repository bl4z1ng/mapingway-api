using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Behaviors.Validation;

public interface IValidationResult
{
    public IDictionary<string, string[]> Failures { get; }

    public static Error ValidationError => new(
        ErrorCode.ValidationError,
        "One or more validation problems occured.");
}
