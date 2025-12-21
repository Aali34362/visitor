namespace Visitor.Core.WebServices.Middleware;

public class TenantMiddleware(RequestDelegate next, TenantService tenantService)
{
    private readonly RequestDelegate _next = next;
    private readonly TenantService _tenantService = tenantService;

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("X-Tenant-ID"))
        {
            string TenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault()!;
            _tenantService.SetTenantId(TenantId);
        }
        await _next(context);
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}