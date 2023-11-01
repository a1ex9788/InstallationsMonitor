using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class InstallationsMonitor
    {
        internal static async Task MonitorAsync(
            string? directory, string? programName, CancellationToken cancellationToken)
        {
            string programNameToUse = programName ?? AskForProgramName();

            Console.WriteLine("Monitoring installation of program '{0}'...", programNameToUse);

            if (directory is null)
            {
                List<Task> tasks = new List<Task>();

                foreach (string drive in DrivesObtainer.GetDrives())
                {
                    tasks.Add(DirectoriesMonitor.MonitorAsync(drive, cancellationToken));
                }

                await Task.WhenAll(tasks);
            }
            else
            {
                await DirectoriesMonitor.MonitorAsync(directory, cancellationToken);
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