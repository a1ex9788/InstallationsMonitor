using InstallationsMonitor.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.Commands.Installations
{
    internal class InstallationsCommandServiceProvider
        : CommandsServiceProvider<IInstallationsCommand, InstallationsCommand>
    {
        internal InstallationsCommandServiceProvider(CancellationToken cancellationToken)
            : base(ConfigureSpecificServices, cancellationToken)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            InstallationsCommand.ConfigureSpecificServices(services);
        }
    }
}