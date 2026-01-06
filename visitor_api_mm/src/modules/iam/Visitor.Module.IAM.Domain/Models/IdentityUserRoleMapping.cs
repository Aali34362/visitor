namespace Visitor.Module.IAM.Domain.Models;

public class IdentityUserRoleMapping : BaseModel
{
    public Guid user_Id { get; set; }
    public Guid role_Id { get; set; }
}
