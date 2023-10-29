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
        public CancellationToken cancellationToken;

        internal MonitorCommand(string? directory, CancellationToken cancellationToken)
        {
            DriveInfo.GetDrives();

            this.directory = directory;
            this.cancellationToken = cancellationToken;
        }

        protected override async Task ExecuteAsync(IServiceProvider serviceProvider)
        {
            if (this.directory is null)
            {
                IEnumerable<string> drives = DriveInfo.GetDrives()
                    .Where(di => di.DriveType == DriveType.Fixed)
                    .Select(di => di.Name);

                foreach (string drive in drives)
                {
                    await DirectoriesMonitor.MonitorAsync(drive, this.cancellationToken);
                }
            }
            else
            {
                await DirectoriesMonitor.MonitorAsync(this.directory, this.cancellationToken);
            }
        }
    }
}