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
                    await this.MonitorDirectoryAsync(drive);
                }
            }
            else
            {
                await this.MonitorDirectoryAsync(this.directory);
            }
        }

        private async Task MonitorDirectoryAsync(string directoryToMonitor)
        {
            using var watcher = new FileSystemWatcher(directoryToMonitor);

            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            try
            {
                while (!this.cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), this.cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // The command is cancelled this way.
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Created: {e.FullPath}");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath}");
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed: {e.OldFullPath} to {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.GetException()}");
        }
    }
}