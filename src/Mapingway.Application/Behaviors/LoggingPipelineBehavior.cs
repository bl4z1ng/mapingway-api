using Mapingway.Common.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mapingway.Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult> 
    where TResult : Result
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResult>> _logger;


    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResult>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResult> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResult> next, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting request {@Request}, {@Time}", 
            typeof(TRequest).Name, DateTime.UtcNow);

        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogInformation("Request failure {@Request}, {@Error} {@Time}", 
                typeof(TRequest).Name, result.Error, DateTime.UtcNow);
        }
        else
        {
            _logger.LogInformation("Completed request {@Request}, {@Time}", 
                typeof(TRequest).Name, DateTime.UtcNow);
        }

        return result;
    }
}