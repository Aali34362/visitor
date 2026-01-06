namespace Visitor.Module.IAM.Domain.Models;

public class IdentityRolePolicyMapping : BaseModel
{
    public Guid policy_Id { get; set; }
    public Guid role_Id { get; set; }
}
