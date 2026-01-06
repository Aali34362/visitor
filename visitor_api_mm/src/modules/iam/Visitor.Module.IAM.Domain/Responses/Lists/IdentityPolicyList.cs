namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPolicyList : BaseResponse
{
    public string name { get; set; }
    public Dictionary<string, string> tags { get; set; }
}
