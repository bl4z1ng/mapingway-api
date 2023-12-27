﻿using Mapingway.Presentation.v1.Auth.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples.Requests.Token;

public class RefreshTokenRequestExample : IExamplesProvider<RefreshTokenRequest>
{
    public RefreshTokenRequest GetExamples()
    {
        return new RefreshTokenRequest
        {
            ExpiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                           "eyJzdWIiOiIyIiwiZW1haWwiOiJtYXgucHl0Z." +
                           "f3NJ8swoeJIJajwdYv_cfC6lPXSLSYuj2d4PaXaLp3A",
            RefreshToken = "PwYuoPqGtW+Jd5aZJWrzUw=="
        };
    }
}