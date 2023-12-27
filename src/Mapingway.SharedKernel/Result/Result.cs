namespace Mapingway.SharedKernel.Result;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }


    public static Result<TValue> Create<TValue>(TValue? value, Error error) where TValue : class
    {
        return value is null ? Failure<TValue>(error) : Success(value);
    }


    public static Result<TValue> Success<TValue>(TValue? value) => new Result<TValue>(true, Error.None, value);

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(false, error, default);

    public static Result Failure(Error error) => new(false, error);
}