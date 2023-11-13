using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class DirectoriesMonitor
    {
        private readonly DatabaseConnection databaseConnection;

        private int? installationId;

        public DirectoriesMonitor(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        internal async Task MonitorAsync(
            string directory, int installationId, CancellationToken cancellationToken)
        {
            Console.WriteLine("Monitoring directory '{0}'...", directory);

            this.installationId = installationId;

            using FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(directory);

            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.IncludeSubdirectories = true;

            fileSystemWatcher.Changed += this.OnChanged;
            fileSystemWatcher.Created += this.OnCreated;
            fileSystemWatcher.Deleted += this.OnDeleted;
            fileSystemWatcher.Renamed += this.OnRenamed;
            fileSystemWatcher.Error += OnError;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // The command is cancelled this way.
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Changed: {e.FullPath}");

            this.databaseConnection.CreateFileOperation(
                new FileChange(e.FullPath, DateTime.Now, this.installationId!.Value));
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Created: {e.FullPath}");

            this.databaseConnection.CreateFileOperation(
                new FileCreation(e.FullPath, DateTime.Now, this.installationId!.Value));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Deleted: {e.FullPath}");

            this.databaseConnection.CreateFileOperation(
                new FileDeletion(e.FullPath, DateTime.Now, this.installationId!.Value));
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Renamed: {e.OldFullPath} to {e.FullPath}");

            this.databaseConnection.CreateFileOperation(
                new FileRenaming(e.FullPath, DateTime.Now, this.installationId!.Value, e.OldFullPath));
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.GetException()}");
        }
    }
}