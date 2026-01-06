namespace Visitor.Module.IAM.Domain.Models;

public class IdentityModule : BaseModel
{
    public string name { get; set; } = null!;
    public string tags { get; set; }
}

