using System.Net;
using Mapingway.Domain.Response;
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
        catch (Exception reqException)
        {
            var timeSpan = DateTime.Now;
            _logger.LogError(reqException, "{Exception} was caught in {Time}", reqException, timeSpan.ToLongTimeString());
            
            await HandleException(reqException, context, timeSpan);
        }
    }

    private async Task HandleException(Exception exception, HttpContext context, DateTime timeSpan)
    {
        int statusCode;
        ExceptionResponse response;
        
        switch (exception)
        {
            case HttpRequestException requestException:
            {
                statusCode = (int)(requestException.StatusCode ?? HttpStatusCode.BadRequest);

                response = new ExceptionResponse
                {
                    StatusCode = statusCode,
                    //TODO: move to constants
                    Message = "Error while processing request from client",
                    ExceptionMessage = requestException.Message,
                    TimeSpan = timeSpan
                };

                break;
            }
            case AggregateException:
            {
                statusCode = (int)HttpStatusCode.InternalServerError;

                response = new ExceptionResponse
                {
                    StatusCode = statusCode,
                    //TODO: move to constants
                    Message = "Error while processing request from client",
                    ExceptionMessage = exception.Message
                };
                break;
            }
            default:
            {
                statusCode = (int)HttpStatusCode.InternalServerError;

                response = new ServerErrorResponse()
                {
                    StatusCode = statusCode,
                    //TODO: move to constants
                    Message = "Error while processing request from client",
                    ExceptionMessage = exception.Message,
                    InnerException = exception.InnerException?.Message,
                    TimeSpan = timeSpan
                };

                break;
            }
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}