namespace Visitor.Core.WebServices.Extensions;

public static partial class HealthCheckExtensions
{
    public static IHostApplicationBuilder AddHealthChecksEndpoint(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
        return builder;
    }
    public static WebApplication MapHealthChecksEndpoint(this WebApplication app)
    {

        // All health checks must pass for app to be considered ready to accept traffic after starting
        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });


        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });


        return app;
    }
}
