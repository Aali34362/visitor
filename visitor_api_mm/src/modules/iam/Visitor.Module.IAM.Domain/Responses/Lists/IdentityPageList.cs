namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPageList : BaseResponse
{
    public Guid parent_Id { get; set; }
    public int page_Level { get; set; }
    public string page_Title { get; set; } = null!;
    public string page_Url { get; set; } = null!;
    public int page_Order { get; set; }
    public string page_Nm { get; set; } = null!;
    public string icon { get; set; }
    public string module_Nm { get; set; }
}
