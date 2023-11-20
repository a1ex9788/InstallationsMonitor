using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using System.Collections.Generic;

namespace InstallationsMonitor.Logic.Commands.Installations.Utilities
{
    public class InstallationsObtainer
    {
        private readonly DatabaseConnection databaseConnection;

        public InstallationsObtainer(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public IEnumerable<Installation> GetInstallations()
        {
            return this.databaseConnection.GetInstallations();
        }
    }
}