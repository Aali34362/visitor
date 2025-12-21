using Visitor.Core.Db.ContextFactory;
using Visitor.Core.Db.DbServices.SqlServices;

namespace Visitor.Core.Db;

public static class ConfigureServices
{
    public static IServiceCollection AddModuleDbContext<TContext>(this IServiceCollection services)
        where TContext : DbContext
        => services.AddDbContext<TContext>((sp, opt) =>
            sp.GetRequiredService<IDatabaseService>().ConfigureDb(opt, typeof(TContext)));

    public static IServiceCollection AddModuleDbContextFactory<TContext>(this IServiceCollection services, bool pooled = false)
        where TContext : DbContext
    {
        if (pooled)
            services.AddPooledDbContextFactory<TContext>((sp, opt) =>
                sp.GetRequiredService<IDatabaseService>().ConfigureDb(opt, typeof(TContext)));
        else
            services.AddDbContextFactory<TContext>((sp, opt) =>
                sp.GetRequiredService<IDatabaseService>().ConfigureDb(opt, typeof(TContext)));
        return services;
    }

    public static void AddSqlServiceContext(this IServiceCollection services, IConfiguration configuration)
    {        
        var provider = configuration["DatabaseConnectionSettings:Provider"];
        services.AddSingleton<IDatabaseService>(sp => provider switch
        {
            "PostgreSql" => ActivatorUtilities.CreateInstance<PostGreSqlService>(sp),
            "SqlServer" => ActivatorUtilities.CreateInstance<SqlServerService>(sp),
            "MySql" => ActivatorUtilities.CreateInstance<MySqlService>(sp),
            "Oracle" => ActivatorUtilities.CreateInstance<OracleService>(sp),
            _ => throw new NotSupportedException("Unsupported provider")
        });
        services.AddHealthChecks();
    }
}
