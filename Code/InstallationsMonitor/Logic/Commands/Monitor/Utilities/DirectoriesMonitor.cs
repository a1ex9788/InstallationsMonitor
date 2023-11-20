using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using Persistence.Contracts;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Logic.Commands.Monitor.Utilities
{
    public class DirectoriesMonitor
    {
        private readonly IDatabaseConnection databaseConnectionEntitiesCreator;
        private readonly IDatabaseFilesChecker databaseFilesChecker;
        private readonly CancellationToken cancellationToken;

        private int? installationId;

        public DirectoriesMonitor(
            IDatabaseConnection databaseConnectionEntitiesCreator,
            IDatabaseFilesChecker databaseFilesChecker,
            CancellationToken cancellationToken)
        {
            this.databaseConnectionEntitiesCreator = databaseConnectionEntitiesCreator;
            this.databaseFilesChecker = databaseFilesChecker;
            this.cancellationToken = cancellationToken;
        }

        public async Task MonitorAsync(string directory, int installationId)
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
                while (!this.cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), this.cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // The command is cancelled this way.
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (this.databaseFilesChecker.IsDatabaseFile(e.FullPath))
            {
                return;
            }

            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Changed: {e.FullPath}");

            this.databaseConnectionEntitiesCreator.CreateFileChange(
                new FileChange(e.FullPath, DateTime.Now, this.installationId!.Value));
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (this.databaseFilesChecker.IsDatabaseFile(e.FullPath))
            {
                return;
            }

            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Created: {e.FullPath}");

            this.databaseConnectionEntitiesCreator.CreateFileCreation(
                new FileCreation(e.FullPath, DateTime.Now, this.installationId!.Value));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (this.databaseFilesChecker.IsDatabaseFile(e.FullPath))
            {
                return;
            }

            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Deleted: {e.FullPath}");

            this.databaseConnectionEntitiesCreator.CreateFileDeletion(
                new FileDeletion(e.FullPath, DateTime.Now, this.installationId!.Value));
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (this.databaseFilesChecker.IsDatabaseFile(e.FullPath))
            {
                return;
            }

            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Renamed: {e.OldFullPath} to {e.FullPath}");

            this.databaseConnectionEntitiesCreator.CreateFileRenaming(
                new FileRenaming(e.FullPath, DateTime.Now, this.installationId!.Value, e.OldFullPath));
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.GetException()}");
        }
    }
}