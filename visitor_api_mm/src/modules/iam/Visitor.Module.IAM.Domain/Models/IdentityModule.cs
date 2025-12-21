namespace Visitor.Module.IAM.Domain.Models;

public class IdentityModule : BaseModel
{
    public string Name { get; set; } = null!;
    public string Tags { get; set; }
}

