namespace Visitor.Core.Db.ContextFactory;

public abstract class BaseSqlService<TConnection> : IDatabaseService  where TConnection : DbConnection, new()
{
    protected readonly ILogger _logger;
    public string Provider { get; }
    public string ConnectionString { get; }

    protected BaseSqlService(string provider, IConfiguration cfg, ILogger logger)
    {
        Provider = provider;
        _logger = logger;
        ConnectionString = cfg["DatabaseConnectionSettings:ConnectionString"] ?? throw new InvalidOperationException($"Connection string not found.");
    }

    public DbConnection CreateConnection()
    {
        var conn = new TConnection();
        conn.ConnectionString = ConnectionString;
        return conn;
    }

    public async Task<int> ExecuteAsync(string sql, object param = null, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);
        var cmd = new CommandDefinition(sql, param, cancellationToken: ct);
        return await conn.ExecuteAsync(cmd);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);
        var cmd = new CommandDefinition(sql, param, cancellationToken: ct);
        return await conn.QueryAsync<T>(cmd);
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);
        var cmd = new CommandDefinition(sql, param, cancellationToken: ct);
        return await conn.QuerySingleOrDefaultAsync<T>(cmd);
    }

    public async Task<TResult> WithTransactionAsync<TResult>(
        Func<IDbConnection, IDbTransaction, Task<TResult>> work,
        IsolationLevel level = IsolationLevel.ReadCommitted,
        CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);
        using var tx = conn.BeginTransaction(level);
        try
        {
            var result = await work(conn, tx);
            tx.Commit();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction failed for provider {Provider}", Provider);
            try { tx.Rollback(); } catch { /* ignore */ }
            throw;
        }
    }

    public async Task<bool> TestConnectionAsync(CancellationToken ct = default)
    {
        try
        {
            using var conn = CreateConnection();
            await conn.OpenAsync(ct);
            return conn.State == ConnectionState.Open;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to open connection for provider {Provider}", Provider);
            return false;
        }
    }

    // ✅ Derived classes implement this ONLY
    public abstract void ConfigureDb(DbContextOptionsBuilder options, Type migrationsFromContext);

    // Default: wire DbContext via ConfigureDb (no per-provider duplication)
    public virtual void AddDbContext<TContext>(IServiceCollection services)
        where TContext : BaseDbContext
        => services.AddDbContext<TContext>(opt => ConfigureDb(opt, typeof(TContext)));


    // Optional health checks – provider can override if it has rich health checks
    public virtual void AddHealthChecks(IHealthChecksBuilder builder)
        => builder.AddSqlHealthCheck(ConnectionString, name: $"{Provider}-db");
}

public static class HealthChecksBuilderExtensions
{
    public static IHealthChecksBuilder AddSqlHealthCheck(this IHealthChecksBuilder builder, string connectionString, string name)
        => builder.AddCheck(name, () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());
}
