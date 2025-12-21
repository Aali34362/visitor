using System.Reflection;

namespace Visitor.Module.DMS.Data.Context;

public partial class DMSServiceContext : BaseDbContext
{
    public DbSet<DocumentCategory> DocumentCategory { get; set; } = null!;
    public DbSet<DocumentType> DocumentType { get; set; } = null!;
    public DbSet<Document> Document { get; set; } = null!;

    public DMSServiceContext(DbContextOptions<DMSServiceContext> options) : base(options)
    {
    }

    protected DMSServiceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("dms");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<DocumentCategory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<DocumentType>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Document>().HasQueryFilter(e => !e.IsDeleted);
    }
}
