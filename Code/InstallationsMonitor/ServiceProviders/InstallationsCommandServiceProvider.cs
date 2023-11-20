using InstallationsMonitor.ServiceProviders.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.ServiceProviders.Installations
{
    public class InstallationsCommandServiceProvider : CommandsServiceProvider
    {
        public InstallationsCommandServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
                : base(ConfigureSpecificServices, cancellationToken, databaseFullName)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddInstallationsCommand();
        }
    }
}