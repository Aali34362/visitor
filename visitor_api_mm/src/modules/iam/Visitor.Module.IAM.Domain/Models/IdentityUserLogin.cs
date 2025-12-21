namespace Visitor.Module.IAM.Domain.Models;

public class IdentityUserLogin : BaseModel
{
    public Guid User_Id { get; set; }
    public Guid Session_Id { get; set; }
    public string Login_Source_Sytem { get; set; }
    public string Login_Source_Sytem_Ip { get; set; }
    public DateTime Login_Date { get; set; }
    public DateTime Logout_Date { get; set; }
}

