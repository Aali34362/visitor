using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Visitor.Module.DMS.Infrastructure;

public static class ConfigureServices
{

    public static IServiceCollection AddDMSInfraModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfraServices(configuration);
        services.AddDBServices(configuration);
        return services;
    }

    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDmsRepository, DmsRepository>();
        services.AddSingleton<IDmsServices, DmsService>();
        return services;
    }

    public static IServiceCollection AddDBServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddModuleDbContextFactory<IAMApplicationDbContext>();
        return services;
    }
}
