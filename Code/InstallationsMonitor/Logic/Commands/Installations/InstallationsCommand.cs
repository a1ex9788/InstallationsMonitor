using InstallationsMonitor.Logic.Commands.Installations.Utilities;

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