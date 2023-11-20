using InstallationsMonitor.Logic.Commands.Installations.Utilities;
using InstallationsMonitor.Logic.Contracts;

namespace InstallationsMonitor.Logic.Commands.Installations
{
    public class InstallationsCommand : IInstallationsCommand
    {
        private readonly InstallationsPrinter installationsPrinter;

        public InstallationsCommand(InstallationsPrinter installationsPrinter)
        {
            this.installationsPrinter = installationsPrinter;
        }

        public void Execute()
        {
            this.installationsPrinter.Print();
        }
    }
}