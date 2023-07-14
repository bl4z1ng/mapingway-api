namespace Mapingway.Common.Result.Validation;

public interface IValidationResult
{
    public static Error ValidationError => 
        new( ErrorCode.ValidationError, "One or more validation problems occured." );

    Error[] Errors { get; }
}