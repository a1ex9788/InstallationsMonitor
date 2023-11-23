using InstallationsMonitor.Domain;
using InstallationsMonitor.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace InstallationsMonitor.Logic.Commands.Installations.Utilities
{
    public class InstallationsPrinter
    {
        private readonly InstallationsObtainer installationsObtainer;
        private readonly CancellationToken cancellationToken;

        public InstallationsPrinter(
            InstallationsObtainer installationsObtainer, CancellationToken cancellationToken)
        {
            this.installationsObtainer = installationsObtainer;
            this.cancellationToken = cancellationToken;
        }

        public void Print()
        {
            IEnumerable<InstallationInfo> installations = this.installationsObtainer.GetInstallations();

            if (!installations.Any())
            {
                Console.WriteLine("No installations monitored yet.");

                return;
            }

            IEnumerable<string> columnNames = new string[] { "Id", "Program name", "Date" };
            TablesCreator tablesCreator = new TablesCreator(columnNames);

            foreach (InstallationInfo installation in installations)
            {
                this.cancellationToken.ThrowIfCancellationRequested();

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