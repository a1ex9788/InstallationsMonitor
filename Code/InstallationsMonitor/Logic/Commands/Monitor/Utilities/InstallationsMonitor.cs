using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstallationsMonitor.Logic.Commands.Monitor.Utilities
{
    public class InstallationsMonitor
    {
        private readonly DirectoriesMonitor directoriesMonitor;
        private readonly IDatabaseConnection databaseConnectionEntitiesCreator;

        public InstallationsMonitor(
            DirectoriesMonitor directoriesMonitor,
            IDatabaseConnection databaseConnectionEntitiesCreator)
        {
            this.directoriesMonitor = directoriesMonitor;
            this.databaseConnectionEntitiesCreator = databaseConnectionEntitiesCreator;
        }

        public async Task MonitorAsync(string? directory, string? programName)
        {
            string programNameToUse = programName ?? AskForProgramName();

            Console.WriteLine("Monitoring installation of program '{0}'...", programNameToUse);

            int installationId = this.databaseConnectionEntitiesCreator.CreateInstallation(
                new Installation(programNameToUse, DateTime.Now));

            if (directory is null)
            {
                List<Task> tasks = new List<Task>();

                foreach (string drive in DrivesObtainer.GetDrives())
                {
                    tasks.Add(this.directoriesMonitor.MonitorAsync(drive, installationId));
                }

                await Task.WhenAll(tasks);
            }
            else
            {
                await this.directoriesMonitor.MonitorAsync(directory, installationId);
            }
        }

        private static string AskForProgramName()
        {
            string? programName = null;

            do
            {
                Console.Write("Introduce the name of the program of the installation to monitor: ");
                string? name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    programName = name;
                }
            }
            while (programName is null);

            return programName;
        }
    }
}