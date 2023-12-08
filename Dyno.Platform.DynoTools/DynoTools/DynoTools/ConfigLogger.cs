using Serilog;
using Serilog.Formatting.Json;
using System.Net.Sockets;


namespace DynoTools
{
    public static class ConfigLogger
    {

        public static LoggerConfiguration LoggerConfig(this LoggerConfiguration LoggerConfig, LogstashConfig logstashConfig)
        {
            return LoggerConfig
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), "logs/logInformation.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("logs/logError.txt", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Udp(logstashConfig.IP, logstashConfig.PORT, AddressFamily.InterNetwork);
        }
    }
}
