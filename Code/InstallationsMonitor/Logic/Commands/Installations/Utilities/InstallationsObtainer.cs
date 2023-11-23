using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using System.Collections.Generic;

namespace InstallationsMonitor.Logic.Commands.Installations.Utilities
{
    public class InstallationsObtainer
    {
        private readonly IDatabaseConnection databaseConnection;

        public InstallationsObtainer(IDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public IEnumerable<InstallationInfo> GetInstallations()
        {
            return this.databaseConnection.GetInstallations();
        }
    }
}