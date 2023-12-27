namespace Mapingway.SharedKernel.Result;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(bool isSuccess, Error error, TValue? value) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue? Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);
}
