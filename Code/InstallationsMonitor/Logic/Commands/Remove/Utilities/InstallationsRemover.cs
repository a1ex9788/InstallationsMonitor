﻿using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using System;

namespace InstallationsMonitor.Logic.Commands.Remove.Utilities
{
    public class InstallationsRemover
    {
        private readonly DatabaseConnection databaseConnection;

        public InstallationsRemover(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public void Remove(int installationId)
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