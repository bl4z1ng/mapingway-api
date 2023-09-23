﻿using Mapingway.API.Internal.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Results.User;

public class LoginResponseExample : IExamplesProvider<LoginResponse>
{
    public LoginResponse GetExamples()
    {   
        return new LoginResponse
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                    "eyJzdWIiOiIyIiwiZW1haWwiOiJtYXgucHl0Z." +
                    "f3NJ8swoeJIJajwdYv_cfC6lPXSLSYuj2d4PaXaLp3A",
            RefreshToken = "PwYuoPqGtW+Jd5aZJWrzUw=="
        };
    }
}