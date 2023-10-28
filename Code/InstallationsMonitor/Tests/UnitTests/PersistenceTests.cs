using FluentAssertions;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using InstallationsMonitor.Persistence;
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
            Installation installation = new Installation
            {
                Id = 1,
                ProgramName = "Program",
                FileOperationsNumber = 4,
            };

            FileChange fileChange = new FileChange
            {
                Id = 1,
                InstallationId = installation.Id,
                FileName = "FileChanged",
                DateTime = DateTime.MinValue,
            };

            FileCreation fileCreation = new FileCreation
            {
                Id = 2,
                InstallationId = installation.Id,
                FileName = "FileCreated",
                DateTime = DateTime.MinValue,
            };

            FileDeletion fileDeletion = new FileDeletion
            {
                Id = 3,
                InstallationId = installation.Id,
                FileName = "FileDeleted",
                DateTime = DateTime.MinValue,
            };

            FileRenaming fileRenaming = new FileRenaming
            {
                Id = 4,
                InstallationId = installation.Id,
                FileName = "FileRenamed",
                OldName = "OldFile",
                DateTime = DateTime.MinValue,
            };

            using AppDbContext appDbContext = DatabaseUtilities.GetTestAppDbContext();
            using DatabaseConnection databaseConnection = new DatabaseConnection(appDbContext);

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