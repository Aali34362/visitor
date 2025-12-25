namespace Visitor.Module.IAM.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddIAMModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppServices(configuration);
        services.AddInfraServices(configuration);
        services.AddDBServices(configuration);
        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CreateIAMProfilers>();
        });
        services.AddThreeTierModuleServices(
           typeof(IIdentityModuleAppService).Assembly,
           typeof(IIdentityModuleBusinessService).Assembly,
           typeof(IIdentityModuleDataLayerService).Assembly
       );
        services.AddScoped<ICascadeDeleteManager, CascadeDeleteManager>();
        return services;
    }

    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Keep infra stuff here (caching, bus, etc.), but avoid duplicating Db wiring.
        services.AddCoreInfraServices(configuration);
        //services.AddIamJwt(configuration);        
        return services;
    }

    public static IServiceCollection AddDBServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleDbContextFactory<IAMApplicationDbContext>();
        return services;
    }
}

