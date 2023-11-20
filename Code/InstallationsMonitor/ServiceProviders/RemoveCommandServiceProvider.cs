using InstallationsMonitor.ServiceProviders.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.ServiceProviders.Remove
{
    public class RemoveCommandServiceProvider : CommandsServiceProvider
    {
        public RemoveCommandServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
                : base(ConfigureSpecificServices, cancellationToken, databaseFullName)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddRemoveCommand();
        }
    }
}