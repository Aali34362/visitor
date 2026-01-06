namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityRoleDetail : BaseResponse
{
    public string name { get; set; } = null!;
    public Dictionary<string, string> tags { get; set; }
}
