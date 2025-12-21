namespace Visitor.Module.IAM.Application.DataLayerServices.Factories;

public class IAMServiceContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _configureDbContext;

    public IAMServiceContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        _configureDbContext = configureDbContext ?? throw new ArgumentNullException(nameof(configureDbContext));
    }

    public IAMApplicationDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<IAMApplicationDbContext>();
        _configureDbContext(optionsBuilder);
        return new IAMApplicationDbContext(optionsBuilder.Options);
    }
}

public static class ApplicationDbContextFactoryExtensions
{
    public static void AddApplicationDbContextFactory(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        => services.AddSingleton(new IAMServiceContextFactory(optionsAction));
}
