using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Mapingway.Infrastructure.Logging;

/// <inheritdoc />
internal class TraceActivityEventEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (Activity.Current == null) return;

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("TraceId", Activity.Current.Id));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SpanId", Activity.Current.SpanId));
    }
}
