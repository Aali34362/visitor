namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityRolePolicyMappingDetail : BaseResponse
{
    public string Policy_Nm { get; set; } = null!;
    public string Role_Nm { get; set; } = null!;
}
