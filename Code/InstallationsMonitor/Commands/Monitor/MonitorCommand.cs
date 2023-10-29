using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class MonitorCommand : Command
    {
        private readonly string? directory;
        private readonly string? programName;
        public CancellationToken cancellationToken;

        internal MonitorCommand(string? directory, string? programName, CancellationToken cancellationToken)
        {
            DriveInfo.GetDrives();

            this.directory = directory;
            this.programName = programName;
            this.cancellationToken = cancellationToken;
        }

        protected override async Task ExecuteAsync(IServiceProvider serviceProvider)
        {
            string? programNameToUse = this.programName ?? AskForProgramName();

            if (this.directory is null)
            {
                IEnumerable<string> drives = DriveInfo.GetDrives()
                    .Where(di => di.DriveType == DriveType.Fixed)
                    .Select(di => di.Name);

                foreach (string drive in drives)
                {
                    await DirectoriesMonitor.MonitorAsync(drive, programNameToUse, this.cancellationToken);
                }
            }
            else
            {
                await DirectoriesMonitor.MonitorAsync(
                    this.directory, programNameToUse, this.cancellationToken);
            }
        }

        private static string AskForProgramName()
        {
            string? programName = null;

            do
            {
                Console.WriteLine("Introduce the name of the program of the installation to monitor.");
                string? name = Console.ReadLine();

                if (!string.IsNullOrEmpty(name))
                {
                    programName = name;
                }
            }
            while (programName is null);

            return programName;
        }
    }
}