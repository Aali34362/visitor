namespace Visitor.Core.WebServices.Middleware;

public class DefaultMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public DefaultMiddleware(RequestDelegate next, IOptionsMonitor<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.CurrentValue;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        httpContext.Response.Headers.Append("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

        var method = httpContext.Request.Method;
        if (HttpMethods.IsPatch(httpContext.Request.Method))
        {
            httpContext.Request.EnableBuffering();
        }

        await _next(httpContext);
    }
}

public static class ConfigurationMiddlewareExtensions
{
    public static IApplicationBuilder UseConfigurationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DefaultMiddleware>();
    }
}
