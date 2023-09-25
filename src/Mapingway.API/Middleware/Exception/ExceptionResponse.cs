﻿using Mapingway.Common;

namespace Mapingway.API.Middleware.Exception;

public class ExceptionResponse : BaseResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
}