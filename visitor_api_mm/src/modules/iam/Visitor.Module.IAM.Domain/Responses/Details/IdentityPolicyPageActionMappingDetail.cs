namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPolicyPageActionMappingDetail : BaseResponse
{
    public string Policy_Nm { get; set; } = null!; 
    public string PageAction_Nm { get; set; } = null!;
}
