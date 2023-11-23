using InstallationsMonitor.Domain;
using InstallationsMonitor.Logic.Utilities;
using InstallationsMonitor.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace InstallationsMonitor.Logic.Commands.Installation.Utilities
{
    public class InstallationPrinter
    {
        private readonly IDatabaseConnection databaseConnection;
        private readonly CancellationToken cancellationToken;

        public InstallationPrinter(
            IDatabaseConnection databaseConnection, CancellationToken cancellationToken)
        {
            this.databaseConnection = databaseConnection;
            this.cancellationToken = cancellationToken;
        }

        public void Print(int installationId)
        {
            InstallationInfo? installation = this.databaseConnection.GetInstallation(installationId);

            if (installation is null)
            {
                Console.WriteLine($"Installation '{installationId}' does not exist.");

                return;
            }

            this.cancellationToken.ThrowIfCancellationRequested();

            IList<FileChange> fileChanges = this.databaseConnection
                .GetFileChanges(installation.Id).ToList();
            IList<FileCreation> fileCreations = this.databaseConnection
                .GetFileCreations(installation.Id).ToList();
            IList<FileDeletion> fileDeletions = this.databaseConnection.
                GetFileDeletions(installation.Id).ToList();
            IList<FileRenaming> fileRenamings = this.databaseConnection
                .GetFileRenamings(installation.Id).ToList();

            if (fileChanges.Count + fileCreations.Count + fileDeletions.Count + fileRenamings.Count == 0)
            {
                Console.WriteLine($"Installation '{installationId}' has not any file operation.");

                return;
            }

            this.cancellationToken.ThrowIfCancellationRequested();

            PrintFileChanges(fileChanges);
            PrintFileCreations(fileCreations);
            PrintFileDeletions(fileDeletions);
            PrintFileRenamings(fileRenamings);
        }

        private static void PrintFileChanges(IList<FileChange> fileChanges)
        {
            if (!fileChanges.Any())
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("File changes:");

            IEnumerable<string> columnNames = new string[] { "FilePath", "Date" };
            TablesCreator tablesCreator = new TablesCreator(columnNames);

            foreach (FileChange fileChange in fileChanges)
            {
                tablesCreator.AddRow(new string[]
                {
                    fileChange.FilePath,
                    fileChange.DateTime.ToString(CultureInfo.InvariantCulture),
                });
            }

            string installationsTable = tablesCreator.Create();

            Console.WriteLine(installationsTable);
        }

        private static void PrintFileCreations(IList<FileCreation> fileCreations)
        {
            if (!fileCreations.Any())
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("File creations:");

            IEnumerable<string> columnNames = new string[] { "FilePath", "Date" };
            TablesCreator tablesCreator = new TablesCreator(columnNames);

            foreach (FileCreation fileCreation in fileCreations)
            {
                tablesCreator.AddRow(new string[]
                {
                    fileCreation.FilePath,
                    fileCreation.DateTime.ToString(CultureInfo.InvariantCulture),
                });
            }

            string installationsTable = tablesCreator.Create();

            Console.WriteLine(installationsTable);
        }

        private static void PrintFileDeletions(IList<FileDeletion> fileDeletions)
        {
            if (!fileDeletions.Any())
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("File deletions:");

            IEnumerable<string> columnNames = new string[] { "FilePath", "Date" };
            TablesCreator tablesCreator = new TablesCreator(columnNames);

            foreach (FileDeletion fileDeletion in fileDeletions)
            {
                tablesCreator.AddRow(new string[]
                {
                    fileDeletion.FilePath,
                    fileDeletion.DateTime.ToString(CultureInfo.InvariantCulture),
                });
            }

            string installationsTable = tablesCreator.Create();

            Console.WriteLine(installationsTable);
        }

        private static void PrintFileRenamings(IList<FileRenaming> fileRenamings)
        {
            if (!fileRenamings.Any())
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("File renamings:");

            IEnumerable<string> columnNames = new string[] { "FilePath", "Date" };
            TablesCreator tablesCreator = new TablesCreator(columnNames);

            foreach (FileRenaming fileRenaming in fileRenamings)
            {
                tablesCreator.AddRow(new string[]
                {
                    fileRenaming.FilePath,
                    fileRenaming.DateTime.ToString(CultureInfo.InvariantCulture),
                });
            }

            string installationsTable = tablesCreator.Create();

            Console.WriteLine(installationsTable);
        }
    }
}