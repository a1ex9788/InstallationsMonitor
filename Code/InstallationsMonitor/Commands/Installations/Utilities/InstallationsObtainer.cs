using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using System.Collections.Generic;

namespace InstallationsMonitor.Commands.Installations.Utilities
{
    internal class InstallationsObtainer
    {
        private readonly DatabaseConnection databaseConnection;

        public InstallationsObtainer(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        internal IEnumerable<Installation> GetInstallations()
        {
            return this.databaseConnection.GetInstallations();
        }
    }
}