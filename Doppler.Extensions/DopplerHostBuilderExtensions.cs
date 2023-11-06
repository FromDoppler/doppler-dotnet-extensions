using Doppler.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace Doppler.Extensions
{
    public static class DopplerHostBuilderExtensions
    {
        public static IHostBuilder UseDopplerConventions(this IHostBuilder builder)
        {
            builder
                .UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration.SetupSeriLog(hostContext.Configuration, hostContext.HostingEnvironment))
                .ConfigureAppConfiguration((hostContext, config) =>
                 {
                     config.AddJsonFile("secret-appsettings.json", true);
                     config.AddJsonFile($"secret-appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);

                     config.AddJsonFile("/run/secrets/appsettings.json", true);
                     config.AddJsonFile($"/run/secrets/appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);

                     config.AddJsonFile("/run/secrets/secret-settings.json", true);
                     config.AddJsonFile("instance-settings.json", true);

                     config.AddJsonFile("appsettings.Secret.json", true);
                     config.AddJsonFile("/run/secrets/appsettings.Secret.json", true);

                     config.AddEnvironmentVariables();
                 });
            return builder;
        }
    }
}