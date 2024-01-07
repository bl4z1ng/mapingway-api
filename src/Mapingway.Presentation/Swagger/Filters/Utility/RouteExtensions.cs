using System.Text.Json;

namespace Mapingway.Presentation.Swagger.Filters.Utility;

public static class RouteExtensions
{
    public static string ToCamelCase(this string route)
    {
        const char separator = '/';

        var parts = route
            .Split(separator)
            .Select(part => part.Trim(separator))
            .Select(part => part.Contains('}')
                ? part
                : JsonNamingPolicy.CamelCase.ConvertName(part));

        return string.Join(separator, parts);
    }
}
