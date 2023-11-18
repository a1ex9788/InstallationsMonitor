using InstallationsMonitor.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class MonitorCommandServiceProvider : CommandsServiceProvider<IMonitorCommand, MonitorCommand>
    {
        internal MonitorCommandServiceProvider(CancellationToken cancellationToken)
            : base(ConfigureSpecificServices, cancellationToken)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            MonitorCommand.ConfigureSpecificServices(services);
        }
    }
}