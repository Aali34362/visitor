namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPolicyDetail : BaseResponse
{
    public string Name { get; set; }
    public Dictionary<string, string> Tags { get; set; }
}
