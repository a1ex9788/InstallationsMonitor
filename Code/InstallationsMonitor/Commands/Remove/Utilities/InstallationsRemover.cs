using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using System;

namespace InstallationsMonitor.Commands.Remove.Utilities
{
    internal class InstallationsRemover
    {
        private readonly DatabaseConnection databaseConnection;

        public InstallationsRemover(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        internal void Remove(int installationId)
        {
            Installation? installation = this.databaseConnection.GetInstallation(installationId);

            if (installation is null)
            {
                Console.Error.WriteLine($"Any installation with id '{installationId}' exists.");

                return;
            }

            this.databaseConnection.RemoveInstallation(installationId);

            Console.WriteLine($"Installation with id '{installationId}' removed.");
        }
    }
}