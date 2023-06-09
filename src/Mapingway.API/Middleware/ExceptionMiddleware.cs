﻿using System.Net;
using Mapingway.Common.Consts;
using Mapingway.Common.Response;
using Newtonsoft.Json;

namespace Mapingway.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;


    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpRequestException reqEx)
        {
            var timeSpan = DateTime.Now;
            _logger.LogError(reqEx, "In {Time} was caught {Exception}", timeSpan.ToLongTimeString(), reqEx);

            await HandleException(
                ExceptionMessage.HTTP_REQUEST_EXCEPTION,
                reqEx.Message,
                reqEx.StatusCode ?? HttpStatusCode.BadRequest,
                context,
                timeSpan
            );
        }
        catch (Exception ex)
        {
            var timeSpan = DateTime.Now;
            _logger.LogError(ex, "In {Time} was caught {Exception}", timeSpan.ToLongTimeString(), ex);
            
            await HandleException(
                ExceptionMessage.SERVER_ERROR_EXCEPTION,
                ex.Message,
                HttpStatusCode.InternalServerError,
                context,
                timeSpan
            );
        }
    }

    private async Task HandleException(string message, string exceptionMessage, HttpStatusCode httpStatusCode, HttpContext context, DateTime timeSpan)
    {
        var statusCode = (int)httpStatusCode;
        var response = new ExceptionResponse
        {
            StatusCode = statusCode,
            Message = message,
            ExceptionMessage = exceptionMessage,
            TimeSpan = timeSpan
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}