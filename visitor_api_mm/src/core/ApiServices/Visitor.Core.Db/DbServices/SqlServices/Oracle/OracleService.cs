using Visitor.Core.Db.ContextFactory;

namespace Visitor.Core.Db.DbServices.SqlServices;

public sealed class OracleService : BaseSqlService<OracleConnection>
{
    public OracleService(IConfiguration cfg, ILogger<OracleService> logger)
        : base("Oracle", cfg, logger) { }

    public override void ConfigureDb(DbContextOptionsBuilder options, Type migrationsFromContext)
    {
        options.UseOracle(ConnectionString, o =>
        {
            o.MigrationsAssembly(migrationsFromContext.Assembly.FullName);
        });
    }

    public override void AddHealthChecks(IHealthChecksBuilder builder)
        => builder.AddOracle(ConnectionString, name: "oracle");
}
