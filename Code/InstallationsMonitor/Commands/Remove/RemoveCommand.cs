using InstallationsMonitor.Commands.Remove.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace InstallationsMonitor.Commands.Remove
{
    internal class RemoveCommand : IRemoveCommand
    {
        private readonly InstallationsRemover installationsRemover;

        public RemoveCommand(InstallationsRemover installationsRemover)
        {
            this.installationsRemover = installationsRemover;
        }

        internal static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddScoped<InstallationsRemover>();
        }

        public void Execute(int installationId)
        {
            this.installationsRemover.Remove(installationId);
        }
    }
}