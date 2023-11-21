using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Tests.UnitTests
{
    [TestClass]
    public class PersistenceTests
    {
        [TestMethod]
        public void ExecuteAllOperations_NewDatabase_OperationsAreCorrectlyPersisted()
        {
            // Arrange.
            Installation installation = new Installation("Program", DateTime.MinValue)
            {
                Id = 1,
            };

            IServiceCollection services = new ServiceCollection();
            services.AddPersistence($"PersistenceTests.{Guid.NewGuid()}.db");
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();

            // Different tests are grouped to improve performance.

            CreateEveryTypeOfEntity_NewDatabase_CreatesEverything(databaseConnection, installation);

            RemoveEveryTypeOfEntity_DatabaseWithData_RemovesEverything(
                databaseConnection, installation.Id);
        }

        private static void CreateEveryTypeOfEntity_NewDatabase_CreatesEverything(
            IDatabaseConnection databaseConnection, Installation installation)
        {
            // Arrange.
            FileChange fileChange = new FileChange("FileChange", DateTime.MinValue, installation.Id);
            FileCreation fileCreation = new FileCreation(
                "FileCreation", DateTime.MinValue, installation.Id);
            FileDeletion fileDeletion = new FileDeletion(
                "FileDeletion", DateTime.MinValue, installation.Id);
            FileRenaming fileRenaming = new FileRenaming(
                "FileRenaming", DateTime.MinValue, installation.Id, "OldFile");

            // Act, assert.
            databaseConnection.CreateInstallation(installation);
            IEnumerable<Installation> installations = databaseConnection.GetInstallations();
            installations.Should().HaveCount(1);
            Installation installationFromDB = installations.Single();
            installationFromDB.Should().Be(installation);

            databaseConnection.CreateFileChange(fileChange);
            databaseConnection.GetFileChanges().Should().HaveCount(1).And.Contain(fileChange);

            databaseConnection.CreateFileCreation(fileCreation);
            databaseConnection.GetFileCreations().Should().HaveCount(1).And.Contain(fileCreation);

            databaseConnection.CreateFileDeletion(fileDeletion);
            databaseConnection.GetFileDeletions().Should().HaveCount(1).And.Contain(fileDeletion);

            databaseConnection.CreateFileRenaming(fileRenaming);
            databaseConnection.GetFileRenamings().Should().HaveCount(1).And.Contain(fileRenaming);
        }

        private static void RemoveEveryTypeOfEntity_DatabaseWithData_RemovesEverything(
            IDatabaseConnection databaseConnection, int installationId)
        {
            // Act, assert.
            databaseConnection.RemoveInstallation(installationId);
            databaseConnection.GetInstallations().Should().BeEmpty();

            databaseConnection.RemoveFileOperations(installationId);
            databaseConnection.GetFileChanges().Should().BeEmpty();
            databaseConnection.GetFileCreations().Should().BeEmpty();
            databaseConnection.GetFileDeletions().Should().BeEmpty();
            databaseConnection.GetFileRenamings().Should().BeEmpty();
        }
    }
}