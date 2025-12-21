using Visitor.Module.IAM.Data.Context;

namespace Visitor.Module.IAM.Migrations.ModelBuilders;

public partial class IamDbContextPostgres : IAMServiceContext
{
    public IamDbContextPostgres(DbContextOptions<IamDbContextPostgres> options) : base(options)
    {
    }
    public IamDbContextPostgres(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Configuration.IdentityModuleConfiguration).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
