namespace Visitor.Core.Domain.Settings;

public class AppSettings
{
    static AppSettings()
    {
        // Initialize any static members if needed
    }

    public static string ContentRootPath { get; set; }
    public static string EnvironmentName { get; set; }
    public static string WebRootPath { get; set; }
    public static JwtSettings JwtSettings { get; set; } = null!;
}


public class DatabaseConnectionSettings
{
    public string Provider { get; set; }
    public string ConnectionString { get; set; }
}

public class JwtSettings
{
    public string Authority { get; set; } = null!;
    public string ValidIssuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public string Key { get; set; } = null!;
}