using FluentAssertions;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using InstallationsMonitor.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstallationsMonitor.Tests.Utilities
{
    internal static class DatabaseChecker
    {
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
                where T : FileOperation
        {
            IList<string> filePathsList = filePaths.ToList();

            IList<T> fileOperations;

            if (typeof(T) == typeof(FileChange))
            {
                fileOperations = databaseConnection.GetFileChanges().Cast<T>().ToList();
            }
            else if (typeof(T) == typeof(FileCreation))
            {
                fileOperations = databaseConnection.GetFileCreations().Cast<T>().ToList();
            }
            else if (typeof(T) == typeof(FileDeletion))
            {
                fileOperations = databaseConnection.GetFileDeletions().Cast<T>().ToList();
            }
            else if (typeof(T) == typeof(FileRenaming))
            {
                fileOperations = databaseConnection.GetFileRenamings().Cast<T>().ToList();
            }
            else
            {
                throw new InvalidOperationException("Unknown file operation.");
            }

            if (checkFileOperationsNumber)
            {
                fileOperations.Should().HaveCount(filePathsList.Count);
            }

            for (int i = 0; i < filePathsList.Count; i++)
            {
                T? fileOperation = fileOperations.SingleOrDefault(
                    fo => fo.FilePath == filePathsList.ElementAt(i)
                        && fo.InstallationId == installationId);

                fileOperation.Should().NotBeNull();
            }
        }
    }
}