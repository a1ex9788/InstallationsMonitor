using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Domain.Base;
using InstallationsMonitor.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstallationsMonitor.Logic.Tests.Utilities
{
    internal static class DatabaseChecker
    {
        internal static Installation CheckInstallation(
            IDatabaseConnection databaseConnection, string programName)
        {
            Installation installation = databaseConnection.GetInstallations().Single();

            installation.ProgramName.Should().Be(programName);

            return installation;
        }

        internal static void CheckInstallations(
            IDatabaseConnection databaseConnection, IEnumerable<string> programNames)
        {
            databaseConnection.GetInstallations().Select(i => i.ProgramName)
                .Should().BeEquivalentTo(programNames);
        }

        internal static void CheckFileOperations<T>(
            IDatabaseConnection databaseConnection,
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
                fileOperations
                    .Where(fo => fo.FilePath == filePathsList.ElementAt(i)
                        && fo.InstallationId == installationId)
                    .Should().HaveCount(1);
            }
        }
    }
}