using Visitor.Core.Db;
using Visitor.Module.DMS;

namespace Visitor.Core.Api;

public static class ConfigureService
{
    public static IServiceCollection AddAllModuleServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var environment = builder.Environment;
        var configuration = builder.Configuration;

        //Add Database Connection
        services.AddSqlServiceContext(configuration);

        //Add Modules Services
        services.AddIAMModule(configuration);
        services.AddDMSModule(configuration);
        return services;
    }
    
}
