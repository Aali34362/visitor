using Visitor.Core.WebServices.Filters;
using Visitor.Core.WebServices.Middleware;

namespace Visitor.Core.WebServices.Extensions;

public static class ApiServicesExtension
{
    public static void AddDefaultApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<PlainTextResultFilter>();
            options.Conventions.Add(new ModuleRouteConvention());
        });
        builder.AddCorsPolicy();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();
        builder.WebHost.ConfigureKestrel(c =>
        {
            c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(15);
        });

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });

    }

    public static void AddApiSettings(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var environment = builder.Environment;

        builder.Configuration
        .SetBasePath(environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

        var appSettings = new AppSettings();
        config.GetSection("AppSettings").Bind(appSettings);
        AppSettings.ContentRootPath = builder.Environment.ContentRootPath;
        AppSettings.EnvironmentName = builder.Environment.EnvironmentName;
        AppSettings.WebRootPath = builder.Environment.WebRootPath;
        builder.Services.AddSingleton(appSettings);
    }

    public static void UseDefaultApiServices(this IApplicationBuilder app)
    {
        app.UseConfigurationMiddleware();
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
