namespace Visitor.Module.IAM.Domain.Models;

public class IdentityUser : BaseModel
{
    public string user_Nm { get; set; } = null!;
    public string first_Nm { get; set; } = null!;
    public string last_Nm { get; set; } = null!;
    public string email { get; set; } = null!;
    public bool is_EmailConfirmed { get; set; } = false;
    public string phone_No { get; set; } = null!;
    public bool is_PhoneNoConfirmed { get; set; } = false;
    public bool twoFactor_Enabled { get; set; } = false;
    public string password_Hash { get; set; } = null!;
}