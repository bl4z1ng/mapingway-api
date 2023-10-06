﻿using Mapingway.Common.Result;

namespace Mapingway.Common.ValidationResult;

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    public Error[] Errors { get; }


    private ValidationResult(Error[] errors) 
        : base(false, IValidationResult.ValidationError, default)
    {
        Errors = errors;
    }


    public static ValidationResult<TValue> WithErrors(Error[] errors)
    {
        return new ValidationResult<TValue>(errors);
    }
}