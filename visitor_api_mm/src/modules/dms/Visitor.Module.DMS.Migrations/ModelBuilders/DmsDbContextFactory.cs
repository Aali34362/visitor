namespace Visitor.Module.DMS.Migrations.ModelBuilders;

internal class DmsDbContextFactory : IDesignTimeDbContextFactory<DmsDbContextPostgres>
{
    public DmsDbContextPostgres CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DmsDbContextPostgres>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Inventory;Username=postgres;Password=root;");
        return new DmsDbContextPostgres(optionsBuilder.Options);
    }
}
