namespace Visitor.Core.Domain.Configurations;

public class BaseService
{
    public static DateTime GetLocalNow() => DateTime.Now;
    public static DateTime GetUtcNow() => DateTime.UtcNow;
    public static string UserName { get; private set; }
    public static string Email { get; private set; }

    private static readonly AsyncLocal<string> _userName = new();
    private static readonly AsyncLocal<string> _email = new();
    private static readonly AsyncLocal<string> _sourceSystem = new();
    public static AsyncLocal<List<string>> _roles { get; private set; } = new();

    public static string GetUserName() => _userName.Value ?? "UnknownUser";
    public static string GetEmail() => _email.Value ?? string.Empty;
    public static string GetSourceSystem() => _sourceSystem.Value ?? string.Empty;
    public static IEnumerable<string> GetRoles() => _roles.Value ?? new List<string>();

    public void SetUserInfo(string userName, string email, IEnumerable<string> roles, string sourceSystem)
    {
        _userName.Value = userName;
        _email.Value = email;
        _roles.Value = roles.ToList();
        _sourceSystem.Value = sourceSystem;
    }

    public static UserInfo UserInfo()
    {
        if (string.IsNullOrEmpty(GetUserName()))
        {
            throw new InvalidOperationException("User information is not available in BaseService.");
        }

        return new UserInfo
        {
            UserName = GetUserName(),
            SourceSystem = GetSourceSystem(),
            Email = GetEmail(),
            Roles = GetRoles(),
            LocalDateTime = GetLocalNow(),
            UtcDateTime = GetUtcNow(),
        };
    }
}
public class UserInfo
{
    public string UserName { get; set; } = string.Empty;
    public string SourceSystem { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime LocalDateTime { get; set; }
    public DateTime UtcDateTime { get; set; }
    public IEnumerable<string> Roles { get; set; } = null!;
}