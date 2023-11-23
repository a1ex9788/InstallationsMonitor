using InstallationsMonitor.Logic.Commands.Installation.Utilities;
using InstallationsMonitor.Logic.Contracts;

namespace InstallationsMonitor.Logic.Commands.Installations
{
    public class InstallationCommand : IInstallationCommand
    {
        private readonly InstallationPrinter installationPrinter;

        public InstallationCommand(InstallationPrinter installationPrinter)
        {
            this.installationPrinter = installationPrinter;
        }

        public void Execute(int installationId)
        {
            this.installationPrinter.Print(installationId);
        }
    }
}