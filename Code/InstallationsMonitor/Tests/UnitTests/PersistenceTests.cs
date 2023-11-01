using FluentAssertions;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstallationsMonitor.Tests.UnitTests
{
    [TestClass]
    public class PersistenceTests
    {
        [TestMethod]
        public void SaveEveryTypeOfEntity_NewDatabase_PersistsEverything()
        {
            // Arrange.
            Installation installation = new Installation("Program", DateTime.MinValue)
            {
                Id = 1,
                FileOperationsNumber = 4,
            };

            FileChange fileChange = new FileChange("FileChanged", DateTime.MinValue, installation.Id);
            FileCreation fileCreation = new FileCreation("FileCreated", DateTime.MinValue, installation.Id);
            FileDeletion fileDeletion = new FileDeletion("FileDeleted", DateTime.MinValue, installation.Id);
            FileRenaming fileRenaming = new FileRenaming(
                "FileRenamed", DateTime.MinValue, installation.Id, "OldFile");

            using DatabaseConnection databaseConnection = DatabaseUtilities.GetTestDatabaseConnection();

            // Act.
            databaseConnection.CreateInstallation(installation);
            databaseConnection.CreateFileOperation(fileChange);
            databaseConnection.CreateFileOperation(fileCreation);
            databaseConnection.CreateFileOperation(fileDeletion);
            databaseConnection.CreateFileOperation(fileRenaming);

            // Assert.
            Installation installationFromDB = databaseConnection.GetInstallations().Single();
            installationFromDB.Should().Be(installation);

            IEnumerable<FileOperation> fileOperations = databaseConnection.GetFileOperations();
            fileOperations.Should().BeEquivalentTo(
                new FileOperation[] { fileChange, fileCreation, fileDeletion, fileRenaming });
        }
    }
}