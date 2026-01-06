namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPolicyPageActionMappingList : BaseResponse
{
    public string policy_Nm { get; set; } = null!;
    public string pageAction_Nm { get; set; } = null!;
}
