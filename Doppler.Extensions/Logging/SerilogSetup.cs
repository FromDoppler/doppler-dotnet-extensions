using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace Doppler.Extensions.Logging
{
    public static class SerilogSetup
    {
        public static LoggerConfiguration SetupSeriLog(
            this LoggerConfiguration loggerConfiguration,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            configuration.ConfigureLoggly(hostEnvironment);

            loggerConfiguration
                .WriteTo.Console()
                .Enrich.WithProperty("Application", hostEnvironment.ApplicationName)
                .Enrich.WithProperty("Environment", hostEnvironment.EnvironmentName)
                .Enrich.WithProperty("Platform", Environment.OSVersion.Platform)
                .Enrich.WithProperty("Runtime", Environment.Version)
                .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration);

            if (hostEnvironment.IsDevelopment())
            {
                loggerConfiguration
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Information)
                    .MinimumLevel.Debug();
            }
            {
                loggerConfiguration
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                    .MinimumLevel.Information()
                    .WriteTo.Loggly();
            }

            return loggerConfiguration;
        }
    }
}