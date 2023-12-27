﻿using Mapingway.Presentation.Controllers.Requests.User;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples.Requests.User;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples()
    {
        return new RegisterRequest
        {
            Email = "max.pyte@gmail.com",
            Password = "Password",
            FirstName = "Max",
            LastName = "Pyte"
        };
    }
}