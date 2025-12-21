namespace Visitor.Module.DMS.Migrations.ModelBuilders;

public partial class DmsDbContextPostgres : DMSServiceContext
{
    public DmsDbContextPostgres(DbContextOptions<DmsDbContextPostgres> options) : base(options)
    {
    }
    public DmsDbContextPostgres(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Configuration.DocumentConfiguration).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
