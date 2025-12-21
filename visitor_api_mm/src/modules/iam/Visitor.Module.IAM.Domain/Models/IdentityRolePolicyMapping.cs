namespace Visitor.Module.IAM.Domain.Models;

public class IdentityRolePolicyMapping : BaseModel
{
    public Guid Policy_Id { get; set; }
    public Guid Role_Id { get; set; }
}
