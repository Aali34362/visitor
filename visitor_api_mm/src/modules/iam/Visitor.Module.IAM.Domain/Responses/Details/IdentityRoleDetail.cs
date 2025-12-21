namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityRoleDetail : BaseResponse
{
    public string Name { get; set; } = null!;
    public Dictionary<string, string> Tags { get; set; }
}
