using InstallationsMonitor.Entities;
using InstallationsMonitor.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace InstallationsMonitor.Commands.Installations.Utilities
{
    internal class InstallationsPrinter
    {
        private readonly InstallationsObtainer installationsObtainer;
        private readonly CancellationToken cancellationToken;

        public InstallationsPrinter(
            InstallationsObtainer installationsObtainer, CancellationToken cancellationToken)
        {
            this.installationsObtainer = installationsObtainer;
            this.cancellationToken = cancellationToken;
        }

        internal void Print()
        {
            IEnumerable<Installation> installations = this.installationsObtainer.GetInstallations();

            IEnumerable<string> columnNames = new string[] { "Id", "Program name", "Date" };

            TablesCreator tablesCreator = new TablesCreator(columnNames);

            foreach (Installation installation in installations)
            {
                tablesCreator.AddRow(new string[]
                {
                    installation.Id.ToString(CultureInfo.InvariantCulture),
                    installation.ProgramName,
                    installation.DateTime.ToString(CultureInfo.InvariantCulture),
                });
            }

            string installationsTable = tablesCreator.Create();

            Console.WriteLine(installationsTable);
        }
    }
}