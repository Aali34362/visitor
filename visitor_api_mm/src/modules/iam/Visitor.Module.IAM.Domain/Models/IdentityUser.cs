namespace Visitor.Module.IAM.Domain.Models;

public class IdentityUser : BaseModel
{
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; } = false;
    public string PhoneNumber { get; set; } = null!;
    public bool IsPhoneNumberConfirmed { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public string PasswordHash { get; set; } = null!;
}