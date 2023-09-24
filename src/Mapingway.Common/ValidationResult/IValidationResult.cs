using Mapingway.Common.Result;

namespace Mapingway.Common.ValidationResult;

public interface IValidationResult
{
    public static Error ValidationError => 
        new( ErrorCode.ValidationError, "One or more validation problems occured." );

    Error[] Errors { get; }
}