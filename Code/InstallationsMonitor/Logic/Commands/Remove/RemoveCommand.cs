using InstallationsMonitor.Logic.Commands.Remove.Utilities;
using InstallationsMonitor.Logic.Contracts;

namespace InstallationsMonitor.Logic.Commands.Remove
{
    public class RemoveCommand : IRemoveCommand
    {
        private readonly InstallationsRemover installationsRemover;

        public RemoveCommand(InstallationsRemover installationsRemover)
        {
            this.installationsRemover = installationsRemover;
        }

        public void Execute(int installationId)
        {
            this.installationsRemover.Remove(installationId);
        }
    }
}