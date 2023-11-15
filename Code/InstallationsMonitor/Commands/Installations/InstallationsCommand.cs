using InstallationsMonitor.Commands.Installations.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace InstallationsMonitor.Commands.Installations
{
    internal class InstallationsCommand : IInstallationsCommand
    {
        private readonly InstallationsPrinter installationsPrinter;

        public InstallationsCommand(InstallationsPrinter installationsPrinter)
        {
            this.installationsPrinter = installationsPrinter;
        }

        internal static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddScoped<InstallationsPrinter>();
            services.AddScoped<InstallationsObtainer>();
        }

        public void Execute()
        {
            this.installationsPrinter.Print();
        }
    }
}