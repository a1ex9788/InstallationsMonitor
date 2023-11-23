using InstallationsMonitor.ServiceProviders.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.ServiceProviders.Delete
{
    public class DeleteCommandServiceProvider : CommandsServiceProvider
    {
        public DeleteCommandServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
                : base(ConfigureSpecificServices, cancellationToken, databaseFullName)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddDeleteCommand();
        }
    }
}