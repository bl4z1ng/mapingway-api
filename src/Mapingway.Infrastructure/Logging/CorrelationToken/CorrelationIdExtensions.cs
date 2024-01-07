using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Mapingway.Infrastructure.Logging.CorrelationToken;

public static class CorrelationIdExtensions
{
    public const string Header = "correlation-id";

    public static string? GetCorrelationId(this IHeaderDictionary headers)
    {
        var token = headers[Header];

        return token == StringValues.Empty ? null : token.ToString();
    }

    public static void AddCorrelationId(this IHeaderDictionary headers, string token)
    {
        headers[Header] = token;
    }
}
