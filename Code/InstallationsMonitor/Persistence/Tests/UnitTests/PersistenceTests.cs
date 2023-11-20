using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using InstallationsMonitor.TestsUtilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace Persistence.Tests.UnitTests
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
            };

            FileChange fileChange = new FileChange("FileChanged", DateTime.MinValue, installation.Id);
            FileCreation fileCreation = new FileCreation("FileCreated", DateTime.MinValue, installation.Id);
            FileDeletion fileDeletion = new FileDeletion("FileDeleted", DateTime.MinValue, installation.Id);
            FileRenaming fileRenaming = new FileRenaming(
                "FileRenamed", DateTime.MinValue, installation.Id, "OldFile");

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();

            // Act.
            databaseConnection.CreateInstallation(installation);
            databaseConnection.CreateFileChange(fileChange);
            databaseConnection.CreateFileCreation(fileCreation);
            databaseConnection.CreateFileDeletion(fileDeletion);
            databaseConnection.CreateFileRenaming(fileRenaming);

            // Assert.
            Installation installationFromDB = databaseConnection.GetInstallations().Single();
            installationFromDB.Should().Be(installation);

            FileChange fileChangeFromDB = databaseConnection.GetFileChanges().Single();
            fileChangeFromDB.Should().Be(fileChange);
            FileCreation fileCreationFromDB = databaseConnection.GetFileCreations().Single();
            fileCreationFromDB.Should().Be(fileCreation);
            FileDeletion fileDeletionFromDB = databaseConnection.GetFileDeletions().Single();
            fileDeletionFromDB.Should().Be(fileDeletion);
            FileRenaming fileRenamingFromDB = databaseConnection.GetFileRenamings().Single();
            fileRenamingFromDB.Should().Be(fileRenaming);
        }
    }
}