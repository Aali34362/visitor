namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityPageList : BaseResponse
{
    public Guid Parent_Id { get; set; }
    public int Page_Level { get; set; }
    public string Page_Title { get; set; } = null!;
    public string Page_Url { get; set; } = null!;
    public int Page_Order { get; set; }
    public string Page_Nm { get; set; } = null!;
    public string Icon { get; set; }
    public string Module_Nm { get; set; }
}
