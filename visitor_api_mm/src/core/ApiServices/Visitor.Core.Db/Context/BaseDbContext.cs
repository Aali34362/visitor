using Visitor.Core.Domain.Configurations;

namespace Visitor.Core.Db.Context;

public abstract class BaseDbContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    private IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            throw new ArgumentNullException(nameof(optionsBuilder), "Value cannot be null. (Parameter 'connectionString')");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()!.ToLower());
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToLower());
            }
        }

    }
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }
    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        return true;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UserInfo user = BaseService.UserInfo();
        DateTime now = BaseService.GetLocalNow();

        var entries = ChangeTracker.Entries<BaseModel>();
        //Add a logic for UnChanged State, what need to do is to capture the first state of the entity when it was added to the context and then if other states are in Unchanged state, we can use that first state to update the entity.

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = now;
            entry.Entity.UpdatedBy = user.UserName;
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = user.UserName;
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                    entry.Property(nameof(BaseModel.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseModel.Act_Ind)).IsModified = false;
                    entry.Property(nameof(BaseModel.IsDeleted)).IsModified = false;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.Act_Ind = 0;
                    entry.Entity.IsDeleted = true;

                    entry.Property(nameof(BaseModel.Act_Ind)).IsModified = true;
                    entry.Property(nameof(BaseModel.IsDeleted)).IsModified = true;
                    entry.Property(nameof(BaseModel.UpdatedAt)).IsModified = true;
                    entry.Property(nameof(BaseModel.UpdatedBy)).IsModified = true;

                    entry.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                    entry.Property(nameof(BaseModel.CreatedAt)).IsModified = false;
                    break;

                ////case EntityState.Unchanged:
                ////    entry.Property(nameof(BaseModel.UpdatedAt)).IsModified = true;
                ////    entry.Property(nameof(BaseModel.UpdatedBy)).IsModified = true;
                ////    entry.State = EntityState.Modified;
                ////    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
