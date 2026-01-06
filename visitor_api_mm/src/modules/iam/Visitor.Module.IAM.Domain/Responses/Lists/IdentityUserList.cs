namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityUserList : BaseResponse
{
    public string user_Nm { get; set; } = null!;
    public string first_Nm { get; set; } = null!;
    public string last_Nm { get; set; } = null!;
    public string email { get; set; } = null!;
    public string phone_No { get; set; } = null!;
}
