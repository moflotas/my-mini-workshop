using Serilog;
using Serilog.Formatting.Compact;

namespace API.Configuration;

public static class Logger
{
    public static Serilog.Core.Logger CreateLogger()
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
            .CreateLogger();

        var loggerForApi = logger.ForContext("Module", "API");

        loggerForApi.Information("Logger configured");

        return logger;
    }
}