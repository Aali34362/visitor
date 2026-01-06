using System.Reflection;

namespace Visitor.Module.Master.Data.Context;

public partial class MasterServiceContext : BaseDbContext
{
    public DbSet<Country> Country { get; set; } = null!;

    public MasterServiceContext(DbContextOptions<MasterServiceContext> options) : base(options)
    {
    }

    protected MasterServiceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("master");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Country>().HasQueryFilter(e => !e.is_Deleted);
    }
}
