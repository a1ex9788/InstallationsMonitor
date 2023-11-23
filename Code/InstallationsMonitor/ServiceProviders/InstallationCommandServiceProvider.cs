using InstallationsMonitor.ServiceProviders.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.ServiceProviders.Installation
{
    public class InstallationCommandServiceProvider : CommandsServiceProvider
    {
        public InstallationCommandServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
                : base(ConfigureSpecificServices, cancellationToken, databaseFullName)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddInstallationCommand();
        }
    }
}