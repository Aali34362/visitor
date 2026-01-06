namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityRolePolicyMappingList : BaseResponse
{
    public string policy_Nm { get; set; } = null!;
    public string role_Nm { get; set; } = null!;
}
