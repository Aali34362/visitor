namespace Visitor.Module.IAM.Domain.Models;

public class IdentityUserRoleMapping : BaseModel
{
    public Guid User_Id { get; set; }
    public Guid Role_Id { get; set; }
}
