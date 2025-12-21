using Microsoft.EntityFrameworkCore.Design;

namespace Visitor.Module.IAM.Migrations.ModelBuilders;

internal class IamDbContextFactory : IDesignTimeDbContextFactory<IamDbContextPostgres>
{
    public IamDbContextPostgres CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IamDbContextPostgres>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Inventory;Username=postgres;Password=root;");
        return new IamDbContextPostgres(optionsBuilder.Options);
    }
}
