namespace Visitor.Module.IAM.Domain.Responses;

public class IdentityUserLoginList : BaseResponse
{
    public Guid User_Id { get; set; }
    public string UserName { get; set; }
    public Guid Session_Id { get; set; }
    public string Login_Source_Sytem { get; set; }
    public string Login_Source_Sytem_Ip { get; set; }
    public DateTime Login_Date { get; set; }
    public DateTime Logout_Date { get; set; }
}
