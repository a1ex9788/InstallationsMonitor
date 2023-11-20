using InstallationsMonitor.ServiceProviders.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.ServiceProviders.Monitor
{
    public class MonitorCommandServiceProvider : CommandsServiceProvider
    {
        public MonitorCommandServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
                : base(ConfigureSpecificServices, cancellationToken, databaseFullName)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddMonitorCommand();
        }
    }
}