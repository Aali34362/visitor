namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityRoleList : BaseResponse
{
    public string Name { get; set; } = null!;
    public Dictionary<string, string> Tags { get; set; }
}
