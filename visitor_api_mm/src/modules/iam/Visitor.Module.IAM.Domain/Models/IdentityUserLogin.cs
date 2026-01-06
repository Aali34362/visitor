namespace Visitor.Module.IAM.Domain.Models;

public class IdentityUserLogin : BaseModel
{
    public Guid user_Id { get; set; }
    public Guid session_Id { get; set; }
    public string login_Source_Sytem { get; set; }
    public string login_Source_Sytem_Ip { get; set; }
    public DateTime login_Dt { get; set; }
    public DateTime logout_Dt { get; set; }
}

