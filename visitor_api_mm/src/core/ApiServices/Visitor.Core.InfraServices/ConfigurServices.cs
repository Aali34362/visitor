using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Visitor.Core.InfraServices;

public static class ConfigurServices
{
    public static IServiceCollection AddCoreInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the validation service
        services.AddScoped<IValidationService, ValidationService>();

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // Register other necessary services here
        // e.g., services.AddScoped<IOtherService, OtherService>();
        return services;
    }

    public static IServiceCollection AddThreeTierModuleServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return services;
    }
}
