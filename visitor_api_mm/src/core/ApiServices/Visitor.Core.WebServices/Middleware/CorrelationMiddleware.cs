namespace Visitor.Core.WebServices.Middleware;

public class CorrelationMiddleware(RequestDelegate next, CorrelationService correlationService, SessionService sessionService, BaseService baseService)
{
    private readonly RequestDelegate _next = next;
    private readonly CorrelationService _correlationService = correlationService;
    private readonly SessionService _sessionService = sessionService;
    private readonly BaseService _baseService = baseService;

    public async Task Invoke(HttpContext context)
    {
        var corrHeader = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
        if (!Guid.TryParse(corrHeader, out var correlationId)) correlationId = Guid.NewGuid();

        _correlationService.SetCorrelationId(correlationId);
        context.Request.Headers["X-Correlation-ID"] = correlationId.ToString();
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Correlation-ID"] = correlationId.ToString();
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            var isAuthenticated = context.User.Identity?.IsAuthenticated == true;

            if (isAuthenticated)
            {
                var claims = context.User.Claims;

                var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "preferred_username")?.Value ?? "UnknownUser";
                userName = context.Request.Headers["X-User-Name"].FirstOrDefault() ?? userName;

                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "UnknownEmail";

                var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                var clientId = claims.FirstOrDefault(c => c.Type == ClaimTypes.System)!.Value ?? "UnknownClientId";

                _baseService.SetUserInfo(userName, email, roles, clientId);
            }

            var sessionIdStr = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.SessionId)?.Value;

            Guid sessionId = Guid.Empty;
            if (Guid.TryParse(sessionIdStr, out var parsed))
                sessionId = parsed;

            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

            if (isAuthenticated && !allowAnonymous)
            {
                if (sessionId == Guid.Empty)
                {
                    await WriteUnauthorizedAsync(context, "Claim Details Invalid.");
                    return;
                }
            }

            _sessionService.SetSessionId(sessionId);

            await _next(context);
        }
    }

    private static Task WriteUnauthorizedAsync(HttpContext context, string detail)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/problem+json";
        // Small ProblemDetails payload (no MVC dependency)
        var body =
            $$"""
            {
              "type": "about:blank",
              "title": "Unauthorized",
              "status": 401,
              "detail": "{{detail}}",
              "traceId": "{{context.TraceIdentifier}}"
            }
            """;
        return context.Response.WriteAsync(body);
    }
}

public static class CorrelationMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationMiddleware>();
    }
}