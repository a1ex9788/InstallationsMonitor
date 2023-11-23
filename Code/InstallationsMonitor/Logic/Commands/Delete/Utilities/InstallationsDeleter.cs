using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using System;

namespace InstallationsMonitor.Logic.Commands.Delete.Utilities
{
    public class InstallationsDeleter
    {
        private readonly IDatabaseConnection databaseConnection;

        public InstallationsDeleter(IDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public void Delete(int installationId)
        {
            Installation? installation = this.databaseConnection.GetInstallation(installationId);

            if (installation is null)
            {
                Console.Error.WriteLine($"Any installation with id '{installationId}' exists.");

                return;
            }

            this.databaseConnection.DeleteFileOperations(installationId);

            this.databaseConnection.DeleteInstallation(installationId);

            Console.WriteLine($"Installation with id '{installationId}' deleted.");
        }
    }
}