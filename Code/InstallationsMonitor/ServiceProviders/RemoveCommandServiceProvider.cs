using InstallationsMonitor.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace InstallationsMonitor.Commands.Remove
{
    internal class RemoveCommandServiceProvider : CommandsServiceProvider<IRemoveCommand, RemoveCommand>
    {
        internal RemoveCommandServiceProvider(CancellationToken cancellationToken)
            : base(ConfigureSpecificServices, cancellationToken)
        {
        }

        private static void ConfigureSpecificServices(IServiceCollection services)
        {
            RemoveCommand.ConfigureSpecificServices(services);
        }
    }
}