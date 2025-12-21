using Visitor.Core.Db.ContextFactory;

namespace Visitor.Core.Db.DbServices.SqlServices;

public sealed class MySqlService : BaseSqlService<MySqlConnection>
{
    public MySqlService(IConfiguration cfg, ILogger<MySqlService> logger)
        : base("MySql", cfg, logger) { }

    public override void ConfigureDb(DbContextOptionsBuilder options, Type migrationsFromContext)
    {
        options.UseMySQL(ConnectionString, my =>
        {
            my.EnableRetryOnFailure(5);
            my.MigrationsAssembly(migrationsFromContext.Assembly.FullName);
        });
    }

    public override void AddHealthChecks(IHealthChecksBuilder builder)
        => builder.AddMySql(ConnectionString, name: "mysql");
}
