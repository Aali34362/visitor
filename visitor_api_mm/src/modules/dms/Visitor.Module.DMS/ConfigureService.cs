using Microsoft.Extensions.Configuration;

namespace Visitor.Module.DMS;

public static class ConfigureService
{
    public static IServiceCollection AddDMSModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDMSApplicationModule(configuration);
        services.AddDMSInfraModule(configuration);
        return services;
    }
}
