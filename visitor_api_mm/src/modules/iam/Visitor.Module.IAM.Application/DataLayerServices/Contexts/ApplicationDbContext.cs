namespace Visitor.Module.IAM.Application.DataLayerServices.Contexts;

public partial class IAMApplicationDbContext : IAMServiceContext
{
    public IAMApplicationDbContext(DbContextOptions<IAMApplicationDbContext> options): base(options)
    {
    }
}
