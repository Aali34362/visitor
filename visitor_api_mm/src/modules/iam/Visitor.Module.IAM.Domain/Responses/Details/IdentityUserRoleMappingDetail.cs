namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityUserRoleMappingDetail : BaseResponse
{
    public string user_Nm { get; set; } = null!;
    public string role_Nm { get; set; } = null!;
}
