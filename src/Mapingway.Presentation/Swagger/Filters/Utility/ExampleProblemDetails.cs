using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Mapingway.Presentation.Swagger.Filters.Utility;

[ExcludeFromCodeCoverage]
public class ExampleProblemDetails : ProblemDetails
{
    public ExampleProblemDetails(
        int statusCode,
        string? type = null,
        string? title = null,
        string? instance = null,
        string? detail = null,
        string? traceId = null,
        IDictionary<string, string[]>? errors = null)
    {
        Status = statusCode;

        Type = type ?? ExampleProblemDetailsProperties.StatusCodeUrl + statusCode;
        Title = title ?? ReasonPhrases.GetReasonPhrase(statusCode);
        Detail = detail;
        Instance = instance;
        TraceId = traceId ?? ExampleProblemDetailsProperties.TraceId;
        Errors = errors;
    }


    public string? TraceId { get; init; }
    public IDictionary<string, string[]>? Errors { get; set; }
}


public static class ExampleProblemDetailsProperties
{
    public const string StatusCodeUrl = "https://httpstatuses.io/";
    public const string TraceId = "00-de0323c58d949722feda08ca733e1e0f-08519d60c4a1874c-00";
}
