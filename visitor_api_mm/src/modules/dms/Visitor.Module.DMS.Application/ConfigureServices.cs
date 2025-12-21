namespace Visitor.Module.DMS.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddDMSApplicationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppServices(configuration);
        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        var asm = typeof(GetDocumentTypeByIdQueryHandler).Assembly;

        services.Scan(scan => scan
            .FromAssemblies(asm)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(FluentValidation.IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                    );

        services.AddMiniMediator();
        services.AddDomainEvent();
        services.Scan(scan => scan
        .FromAssemblies(typeof(DocumentTypeCreatedEvent).Assembly)
        .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
