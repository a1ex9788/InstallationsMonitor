using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class DirectoriesMonitor
    {
        internal static async Task MonitorAsync(
            string directoryToMonitor, CancellationToken cancellationToken)
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