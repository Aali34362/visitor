namespace Visitor.Core.Domain.Base;

public class BaseResponse
{
    public Guid Id { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int Act_Ind { get; set; }
    public string UpdatedBy { get; set; } = "Admin";
}
