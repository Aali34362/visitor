namespace Visitor.Module.IAM.Domain.Models;

public class IdentityRole : BaseModel
{
    public string Name { get; set; } = null!;
    public string Tags { get; set; }
}

