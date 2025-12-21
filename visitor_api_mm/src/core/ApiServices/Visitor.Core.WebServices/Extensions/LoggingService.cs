using Serilog;
using Serilog.Events;
using System.Reflection;

namespace Visitor.Core.WebServices.Extensions;

public static class LoggingService
{
    public static void ConfigureLogger()
    {
        string logFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Inventory_Logger");
        Directory.CreateDirectory(logFolderPath);

        string logFilePath = Path.Combine(logFolderPath, $"log_{DateTime.Now:yyyy-MM-dd}.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Suppress framework logs
            .Enrich.WithProperty("Application", "InventorySystem")
            .Enrich.FromLogContext()
            .WriteTo.Console() // Allows console logging
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day,
                          retainedFileCountLimit: 30, // Keeps last 30 days of logs
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
            .CreateLogger();
    }
    public static void LogVerbose(string message) => Log.Verbose(message);
    public static void LogDebug(string message) => Log.Debug(message);
    public static void LogInfo(string message) => Log.Information(message);
    public static void LogWarning(string message) => Log.Warning(message);
    public static void LogError(string message, Exception? ex = null) => Log.Error(ex, message);
    public static void LogFatal(string message, Exception? ex = null) => Log.Fatal(ex, message);

    // Shutdown method to ensure logs are properly flushed
    public static void ShutdownLogger()
    {
        Log.CloseAndFlush();
    }
}
