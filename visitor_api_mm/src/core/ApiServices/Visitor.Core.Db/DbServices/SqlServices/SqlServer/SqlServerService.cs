using Visitor.Core.Db.ContextFactory;

namespace Visitor.Core.Db.DbServices.SqlServices;

public sealed class SqlServerService : BaseSqlService<SqlConnection>
{
    public SqlServerService(IConfiguration cfg, ILogger<SqlServerService> logger)
        : base("SqlServer", cfg, logger) { }

    public override void ConfigureDb(DbContextOptionsBuilder options, Type migrationsFromContext)
    {
        options.UseSqlServer(ConnectionString, sql =>
        {
            sql.EnableRetryOnFailure(5);
            sql.MigrationsAssembly(migrationsFromContext.Assembly.FullName);
        });
    }

    public override void AddHealthChecks(IHealthChecksBuilder builder)
        => builder.AddSqlServer(ConnectionString, name: "sqlserver");
}
