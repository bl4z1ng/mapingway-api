﻿using Mapingway.SharedKernel.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples;

public class Register400ErrorResultExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(DefaultErrorCode.InvalidCredentials, "User with such email is already registered");
    }
}