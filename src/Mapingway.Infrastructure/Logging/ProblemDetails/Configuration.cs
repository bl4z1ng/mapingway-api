﻿using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace Mapingway.Infrastructure.Logging.ProblemDetails;

[ExcludeFromCodeCoverage]
public static class Configuration
{
    public static void ConfigureProblemDetails(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (_, _) => environment.IsDevelopment();
            options.ShouldLogUnhandledException = (_, _, _) => true;
            //TODO: add exception logging and uncomment this
            options.RethrowAll();

            options
                .MapFluentValidationException()
                .MapExceptions();

        }).AddProblemDetailsConventions();
    }

    private static void MapExceptions(this ProblemDetailsOptions options)
    {
        options.Map<Exception>(exception =>
        {
            if (exception.InnerException is not null) exception = exception.InnerException;

            var statusCode = exception switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                InvalidOperationException => StatusCodes.Status500InternalServerError,
                //Global mapping of HttpClient exceptions, needs further investigation
                HttpRequestException => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status500InternalServerError
            };
            return StatusCodeProblemDetails.Create(statusCode);
        });
    }

    private static ProblemDetailsOptions MapFluentValidationException(this ProblemDetailsOptions options)
    {
        options.Map<ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var errors = ex.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.ErrorMessage).ToArray());

            var problemDetails = factory.CreateValidationProblemDetails(ctx, errors);
            problemDetails.Instance = ctx.Request.Path.ToUriComponent();

            return problemDetails;
        });

        return options;
    }
}
