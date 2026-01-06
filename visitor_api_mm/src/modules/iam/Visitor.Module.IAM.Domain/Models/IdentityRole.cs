namespace Visitor.Module.IAM.Domain.Models;

public class IdentityRole : BaseModel
{
    public string name { get; set; } = null!;
    public string tags { get; set; }
}

