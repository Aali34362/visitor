namespace Visitor.Core.WebServices.Middleware;

public class CustomCorsMiddleware
{
    private readonly RequestDelegate _next;

    public CustomCorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        httpContext.Response.Headers.Append("Access-Control-Allow-Methods", "POST, GET, PUT, PATCH, DELETE, OPTIONS");
        httpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, Access-Control-Max-Age");
        httpContext.Response.Headers.Append("Access-Control-Allow-Credentials", "true");

        if (httpContext.Request.Method == HttpMethods.Options)
        {
            httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
            return;
        }

        if (HttpMethods.IsPatch(httpContext.Request.Method))
            httpContext.Request.EnableBuffering();

        await _next(httpContext);
    }
}

public static class CustomCorsMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomCorsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomCorsMiddleware>();
    }
}