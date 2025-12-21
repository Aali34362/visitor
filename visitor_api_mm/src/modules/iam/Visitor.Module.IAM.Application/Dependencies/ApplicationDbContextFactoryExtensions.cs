using Visitor.Module.IAM.Application.DataLayerServices.Factories;

namespace Visitor.Module.IAM.Application.Dependencies;

public static class ApplicationDbContextFactoryExtensions
{
    public static void AddApplicationDbContextFactory(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddSingleton(new IAMServiceContextFactory(optionsAction));
    }
}