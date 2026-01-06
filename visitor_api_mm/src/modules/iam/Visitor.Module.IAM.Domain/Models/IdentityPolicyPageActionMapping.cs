namespace Visitor.Module.IAM.Domain.Models;

public class IdentityPolicyPageActionMapping : BaseModel
{
    public Guid policy_Id { get; set; }
    public Guid pageAction_Id { get; set; }
}
