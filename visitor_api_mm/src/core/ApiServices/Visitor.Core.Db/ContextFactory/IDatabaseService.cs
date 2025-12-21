namespace Visitor.Core.Db.ContextFactory;

public interface IDatabaseService
{
    string Provider { get; }
    string ConnectionString { get; }

    // Low-level connection
    DbConnection CreateConnection();

    // Dapper helpers
    Task<int> ExecuteAsync(string sql, object param = null, CancellationToken ct = default);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CancellationToken ct = default);
    Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, CancellationToken ct = default);

    // Transaction utility
    Task<TResult> WithTransactionAsync<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> work, IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default);

    // Quick connectivity check
    Task<bool> TestConnectionAsync(CancellationToken ct = default);

    // ✅ Provider-agnostic EF configuration hook (used by modules)
    void ConfigureDb(DbContextOptionsBuilder options, Type migrationsFromContext);

    // EF Core DI wiring (provider specific in derived class)
    void AddDbContext<TContext>(IServiceCollection services) where TContext : BaseDbContext;

    // Optional: health checks
    void AddHealthChecks(IHealthChecksBuilder builder);
}
