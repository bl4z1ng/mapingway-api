﻿using System.Net;
using Mapingway.Infrastructure.Authentication.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Mapingway.API.Middleware.Exception;

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
                ExceptionMessage.HttpRequestException,
                reqEx.Message,
                reqEx.StatusCode ?? HttpStatusCode.BadRequest,
                context,
                timeSpan
            );
        }
        catch (RefreshTokenUsedException ex)
        {
            var timeSpan = DateTime.Now;
            _logger.LogError(ex, "In {Time} was caught exception: {Exception}", timeSpan.ToLongTimeString(), ex);
            
            // TODO: remove ex.Message, make specific message without token expose
            await HandleException(
                ExceptionMessage.UsedRefreshTokenException,
                ex.Message,
                HttpStatusCode.Unauthorized,
                context,
                timeSpan
            );
        }
        catch (SecurityTokenException ex)
        {
            var timeSpan = DateTime.Now;
            _logger.LogError(ex, "In {Time} was caught exception: {Exception}", timeSpan.ToLongTimeString(), ex);
            
            await HandleException(
                ExceptionMessage.InvalidAccessToken,
                ex.Message,
                HttpStatusCode.Unauthorized,
                context,
                timeSpan
            );
        }
        catch (System.Exception ex)
        {
            var timeSpan = DateTime.Now;
            _logger.LogError(ex, "In {Time} was caught exception: {Exception}", timeSpan.ToLongTimeString(), ex);
            
            await HandleException(
                ExceptionMessage.ServerErrorException,
                ex.Message,
                HttpStatusCode.InternalServerError,
                context,
                timeSpan
            );
        }
    }


    private async Task HandleException(
        string message, 
        string exceptionMessage, 
        HttpStatusCode httpStatusCode, 
        HttpContext context, 
        DateTime timeSpan)
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