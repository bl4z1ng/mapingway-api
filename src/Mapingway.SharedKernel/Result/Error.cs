namespace Mapingway.SharedKernel.Result;

public record Error
{
    public Error(DefaultErrorCode code, string? message = null)
    {
        Code = code.ToString();
        Message = message;
    }

    public Error(string code, string? message = null)
    {
        Code = code;
        Message = message;
    }

    public static Error None => new(DefaultErrorCode.None, string.Empty);

    public string Code { get; }

    public string? Message { get; }

    public static implicit operator string(Error? error) => error is null || error.Code == DefaultErrorCode.None.ToString()
        ? string.Empty
        : error.Code;
}
