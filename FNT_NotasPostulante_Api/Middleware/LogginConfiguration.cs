using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

namespace FNT_NotasPostulante_Api.Middleware;

public static class LogginConfiguration
{
    public static Logger CreateBootstrapLogger()
    {
        return new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }

    public static Logger AddLoggerConfiguration(IConfiguration applicationConfiguration)
    {
        var configuration = new LoggerConfiguration()
            .ReadFrom.Configuration(applicationConfiguration)
            .Enrich.FromLogContext();

        var connectionString = applicationConfiguration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.ConnectionString = connectionString;
            configuration.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
        }

        return configuration.CreateLogger();
    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(Log.Logger, dispose: false);
    }
}
