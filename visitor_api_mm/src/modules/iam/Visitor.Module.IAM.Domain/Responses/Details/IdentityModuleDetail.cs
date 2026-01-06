namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityModuleDetail : BaseResponse
{
    public string name { get; set; } = null!;
    public Dictionary<string,string> tags { get; set; }
}
