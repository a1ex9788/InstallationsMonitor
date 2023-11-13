using FluentAssertions;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using InstallationsMonitor.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InstallationsMonitor.Tests.Utilities
{
    internal static class DatabaseUtilities
    {
        internal static DatabaseConnection GetTestDatabaseConnection()
        {
            string testDatabaseFullName = Path.Combine(
                TempPathUtilities.GetTempDirectory(), "TestDatabase.db");

            DatabaseOptions databaseOptions = new DatabaseOptions(testDatabaseFullName);
            AppDbContext appDbContext = new AppDbContext(databaseOptions);
            DatabaseConnection databaseConnection = new DatabaseConnection(appDbContext);

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();

            return databaseConnection;
        }

        internal static Installation CheckInstallation(
            DatabaseConnection databaseConnection, string programName)
        {
            Installation installation = databaseConnection.GetInstallations().Single();

            installation.ProgramName.Should().Be(programName);

            return installation;
        }

        internal static void CheckFileOperations<T>(
            DatabaseConnection databaseConnection,
            int installationId,
            IEnumerable<string> filePaths,
            bool checkFileOperationsNumber = true)
        {
            IList<string> filePathsList = filePaths.ToList();

            IList<FileOperation> fileOperations = databaseConnection.GetFileOperations().ToList();

            if (checkFileOperationsNumber)
            {
                fileOperations.Should().HaveCount(filePathsList.Count);
            }

            for (int i = 0; i < filePathsList.Count; i++)
            {
                FileOperation? fileOperation = fileOperations.SingleOrDefault(
                    fo => fo.GetType() == typeof(T)
                        && fo.FileName == filePathsList.ElementAt(i)
                        && fo.InstallationId == installationId);

                fileOperation.Should().NotBeNull();
            }
        }
    }
}