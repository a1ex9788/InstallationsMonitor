using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Domain.Base;
using InstallationsMonitor.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstallationsMonitor.TestsUtilities
{
    public static class DatabaseChecker
    {
        public static Installation CheckInstallation(
            IDatabaseConnection databaseConnection, string programName)
        {
            Installation installation = databaseConnection.GetInstallations().Single();

            installation.ProgramName.Should().Be(programName);

            return installation;
        }

        public static void CheckInstallations(
            IDatabaseConnection databaseConnection, IEnumerable<string> programNames)
        {
            IEnumerable<Installation> installations = databaseConnection.GetInstallations();

            installations.Should().HaveSameCount(programNames);

            foreach (string programName in programNames)
            {
                installations.Where(i => i.ProgramName == programName).Should().HaveCount(1);
            }
        }

        public static void CheckFileOperations<T>(
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