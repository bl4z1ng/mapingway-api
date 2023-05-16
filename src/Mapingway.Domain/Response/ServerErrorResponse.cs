namespace Mapingway.Domain.Response;

public class ServerErrorResponse : ExceptionResponse
{
    public string? InnerException { get; set; }
}