namespace Visitor.Core.Domain.Base;

public class BaseResponse
{
    public Guid id { get; set; }
    public DateTime updated_At { get; set; } = DateTime.Now;
    public int act_Ind { get; set; }
    public string updated_By { get; set; } = "Admin";
}
