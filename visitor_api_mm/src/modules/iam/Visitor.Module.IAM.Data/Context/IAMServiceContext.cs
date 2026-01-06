using System.Reflection;

namespace Visitor.Module.IAM.Data.Context;

public partial class IAMServiceContext : BaseDbContext
{
    public DbSet<IdentityModule> IdentityModule { get; set; } = null!;
    public DbSet<IdentityPage> IdentityPage { get; set; } = null!;
    public DbSet<IdentityPageAction> IdentityPageAction { get; set; } = null!;
    public DbSet<IdentityPolicy> IdentityPolicy { get; set; } = null!;
    public DbSet<IdentityPolicyPageActionMapping> IdentityPolicyPageActionMapping { get; set; } = null!;
    public DbSet<IdentityRole> IdentityRole { get; set; } = null!;
    public DbSet<IdentityRolePolicyMapping> IdentityRolePolicyMapping { get; set; } = null!;
    public DbSet<IdentityUser> IdentityUser { get; set; } = null!;
    public DbSet<IdentityUserLogin> IdentityUserLogin { get; set; } = null!;
    public DbSet<IdentityUserRoleMapping> IdentityUserRoleMapping { get; set; } = null!;

    public IAMServiceContext(DbContextOptions<IAMServiceContext> options) : base(options)
    {
    }

    protected IAMServiceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("iam");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<IdentityModule>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityPage>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityPageAction>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityPolicy>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityPolicyPageActionMapping>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityRole>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityRolePolicyMapping>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityUser>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityUserLogin>().HasQueryFilter(e => !e.is_Deleted);
        modelBuilder.Entity<IdentityUserRoleMapping>().HasQueryFilter(e => !e.is_Deleted);
    }
}