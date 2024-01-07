using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace Mapingway.Presentation.Swagger.Filters.Utility;

/// <summary>
/// WARNING! Uses reflection for not ProblemDetails type, just a prototype to support test objects
/// </summary>
[ExcludeFromCodeCoverage]
public static class ClassToOpenApiObjectMapper
{
    public static OpenApiObject Map<T>(T obj, bool namesToLowerInvariant = false) where T : class
    {
        if (obj is ProblemDetails problemDetails)
        {
            return ErrorToOpenApiObject(problemDetails, namesToLowerInvariant);
        }

        var props = obj
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public);

        var openApiObject = new OpenApiObject();
        foreach (var property in props)
        {
            var type = property.PropertyType;
            if (!type.IsSimple()) continue;

            var typeCode = Type.GetTypeCode(type);

            var name = namesToLowerInvariant ? property.Name.ToLowerInvariant() : property.Name;

            openApiObject[name] =
                typeCode switch
                {
                    TypeCode.String => new OpenApiString(property.GetValue(obj)!.ToString()),
                    TypeCode.Boolean => new OpenApiString(property.GetValue(obj)!.ToString()),

                    TypeCode.Byte => new OpenApiInteger((byte)property.GetValue(obj)!),
                    TypeCode.Int16 => new OpenApiInteger((short)property.GetValue(obj)!),
                    TypeCode.Int32 => new OpenApiInteger((int)property.GetValue(obj)!),
                    TypeCode.UInt16 => new OpenApiInteger((ushort)property.GetValue(obj)!),

                    TypeCode.Single => new OpenApiFloat((float)property.GetValue(obj)!),
                    TypeCode.Double => new OpenApiFloat((float)property.GetValue(obj)!),
                    _ => new OpenApiNull()
                };
        }

        return openApiObject;
    }


    //TODO: REMOVE THE FUCKING MAPPER, YOU ONLY NEED ONE MAPPING


    private static OpenApiObject ErrorToOpenApiObject(ProblemDetails problemDetails, bool namesToLowerInvariant = false)
    {
        return new OpenApiObject
        {
            [namesToLowerInvariant ? nameof(ProblemDetails.Type) : nameof(ProblemDetails.Type).ToLowerInvariant()] = new OpenApiString(problemDetails.Type),
            [namesToLowerInvariant ? nameof(ProblemDetails.Title) : nameof(ProblemDetails.Title).ToLowerInvariant()] = new OpenApiString(problemDetails.Title),
            [namesToLowerInvariant ? nameof(ProblemDetails.Status) : nameof(ProblemDetails.Status).ToLowerInvariant()] = new OpenApiInteger((int)problemDetails.Status!),
            [namesToLowerInvariant ? "TraceId" : "traceId"] = new OpenApiString(problemDetails.Extensions["TraceId"]!.ToString())
        };
    }

    private static bool IsSimple(this Type type)
    {
        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal);
    }
}
