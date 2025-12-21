namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityUserRoleMappingList : BaseResponse
{
    public string User_Nm { get; set; } = null!;
    public string Role_Nm { get; set; } = null!;
}
