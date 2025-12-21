using Serilog;

namespace Visitor.Core.WebServices.Extensions;

public static class SerilogLoggingExtenstion
{
    public static void AddSerilogConfiguration(this WebApplicationBuilder builder)
    {
        LoggingService.ConfigureLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog();
        });
    }
}