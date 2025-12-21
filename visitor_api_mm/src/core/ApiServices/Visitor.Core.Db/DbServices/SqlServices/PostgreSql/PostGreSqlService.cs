using Visitor.Core.Db.ContextFactory;

namespace Visitor.Core.Db.DbServices.SqlServices;

public sealed class PostGreSqlService : BaseSqlService<NpgsqlConnection>
{
    public PostGreSqlService(IConfiguration cfg, ILogger<PostGreSqlService> logger)
        : base("PostgreSQL", cfg, logger) { }

    public override void ConfigureDb(DbContextOptionsBuilder options, Type migrationsFromContext)
    {
        options.UseNpgsql(ConnectionString, npg =>
        {
            ////npg.EnableRetryOnFailure(5);
            npg.MigrationsAssembly(migrationsFromContext.Assembly.FullName);
        });
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public override void AddHealthChecks(IHealthChecksBuilder builder)
        => builder.AddNpgSql(ConnectionString, name: "postgresql");
}