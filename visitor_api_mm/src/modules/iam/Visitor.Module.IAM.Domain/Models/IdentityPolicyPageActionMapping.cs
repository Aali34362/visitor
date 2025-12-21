namespace Visitor.Module.IAM.Domain.Models;

public class IdentityPolicyPageActionMapping : BaseModel
{
    public Guid Policy_Id { get; set; }
    public Guid PageAction_Id { get; set; }
}
