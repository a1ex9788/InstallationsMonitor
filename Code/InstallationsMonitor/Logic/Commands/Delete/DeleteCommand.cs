using InstallationsMonitor.Logic.Commands.Delete.Utilities;
using InstallationsMonitor.Logic.Contracts;

namespace InstallationsMonitor.Logic.Commands.Delete
{
    public class DeleteCommand : IDeleteCommand
    {
        private readonly InstallationsDeleter installationsDeleter;

        public DeleteCommand(InstallationsDeleter installationsDeleter)
        {
            this.installationsDeleter = installationsDeleter;
        }

        public void Execute(int installationId)
        {
            this.installationsDeleter.Delete(installationId);
        }
    }
}