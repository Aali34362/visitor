namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityUserRoleMappingDetail : BaseResponse
{
    public string User_Nm { get; set; } = null!;
    public string Role_Nm { get; set; } = null!;
}
