using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Persistence.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Persistence.Tests.UnitTests
{
    [TestClass]
    public class PersistenceTests
    {
        [TestMethod]
        public void ExecuteAllOperations_NewDatabase_OperationsAreCorrectlyPersisted()
        {
            // Arrange.
            IServiceCollection services = new ServiceCollection();
            services.AddPersistence($"PersistenceTests.{Guid.NewGuid()}.db");
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            DatabaseContext databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            // All tests are grouped in only one to improve performance.

            // Act, Assert.
            CreationTests creationTests = new CreationTests(databaseConnection, databaseContext);
            IEnumerable<Installation> installations = creationTests.TestCreateInstallation();
            IEnumerable<FileChange> fileChanges = creationTests.TestCreateFileChanges(installations);
            IEnumerable<FileCreation> fileCreations = creationTests.TestCreateFileCreations(installations);
            IEnumerable<FileDeletion> fileDeletions = creationTests.TestCreateFileDeletions(installations);
            IEnumerable<FileRenaming> fileRenamings = creationTests.TestCreateFileRenamings(installations);

            ObtentionTests obtentionTests = new ObtentionTests(databaseConnection);
            obtentionTests.TestGetInstallations(installations);
            obtentionTests.TestGetInstallation(installations);
            obtentionTests.TestGetFileChanges(fileChanges);
            obtentionTests.TestGetFileCreations(fileCreations);
            obtentionTests.TestGetFileDeletions(fileDeletions);
            obtentionTests.TestGetFileRenamings(fileRenamings);

            DeletionTests deletionTests = new DeletionTests(databaseConnection, databaseContext);
            deletionTests.TestDeleteInstallation(installations);
            deletionTests.TestDeleteFileOperations(
                installations, fileChanges, fileCreations, fileDeletions, fileRenamings);
        }
    }
}