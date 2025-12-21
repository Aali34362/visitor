namespace Visitor.Module.IAM.Domain.Models;

public class IdentityPage : BaseModel
{
    public Guid Parent_Id { get; set; }
    public int Page_Level { get; set; }
    public string Page_Title { get; set; } = null!;
    public string Page_Url { get; set; } = null!;
    public int Page_Order { get; set; }
    public string Page_Nm { get; set; } = null!;
    public string Icon { get; set; }
    public Guid Module_Id { get; set; }
}