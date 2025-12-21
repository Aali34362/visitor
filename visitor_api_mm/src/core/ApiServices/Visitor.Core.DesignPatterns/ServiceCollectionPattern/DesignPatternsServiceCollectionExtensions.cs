using Visitor.Core.DesignPatterns.EventPattern;
using Visitor.Core.DesignPatterns.MediatRPattern;
using Microsoft.Extensions.DependencyInjection;

namespace Visitor.Core.DesignPatterns.ServiceCollectionPattern;

public static class DesignPatternsServiceCollectionExtensions
{
    public static IServiceCollection AddDesignPatterns(this IServiceCollection services)
    {
        // This project mostly provides abstractions. Register concrete defaults here if any.
        // Example: services.AddSingleton<IPolicyProvider, DefaultPolicyProvider>(); // but usually in API layer        
        return services;
    }

    public static IServiceCollection AddMiniMediator(this IServiceCollection services)
            => services.AddScoped<IMediatRDispatcher, MediatRDispatcher>();

    public static IServiceCollection AddDomainEvent(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventSink, DomainEventSink>();
        services.AddScoped<IDomainEventCollector>(sp => (DomainEventSink)sp.GetRequiredService<IDomainEventSink>());
        services.AddScoped<IEventBus, EventBus>();
        return services;
    }
}