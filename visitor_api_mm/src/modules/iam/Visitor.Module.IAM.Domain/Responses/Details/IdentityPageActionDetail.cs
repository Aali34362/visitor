namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPageActionDetail : BaseResponse
{     
    public string name { get; set; }
    public string action { get; set; }
    public string access_Level { get; set; }
    public string page_Url { get; set; }
    public string page_Nm { get; set; } = null!;
}
