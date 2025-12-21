namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPageActionList : BaseResponse
{
    public string Name { get; set; }
    public string Action { get; set; } 
    public string AccessLevel { get; set; }
    public string PageUrl { get; set; }
    public string Page_Nm { get; set; } = null!;
}
