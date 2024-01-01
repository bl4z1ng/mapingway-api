namespace Mapingway.SharedKernel.Result;

public record Error
{
    public Error(ErrorCode code, string message)
    {
        Code = code.ToString();
        Message = message;
    }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error None => new(ErrorCode.None, string.Empty);

    public string Code { get; }

    public string Message { get; }

    public static implicit operator string(Error? error) => error is null || error.Code == ErrorCode.None.ToString()
        ? string.Empty
        : error.Code;
}
