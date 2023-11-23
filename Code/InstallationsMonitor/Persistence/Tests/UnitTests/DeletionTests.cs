using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Persistence.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Tests.UnitTests
{
    internal class DeletionTests
    {
        private readonly IDatabaseConnection databaseConnection;
        private readonly DatabaseContext databaseContext;

        internal DeletionTests(IDatabaseConnection databaseConnection, DatabaseContext databaseContext)
        {
            this.databaseConnection = databaseConnection;
            this.databaseContext = databaseContext;
        }

        internal void TestDeleteInstallation(IEnumerable<InstallationInfo> installations)
        {
            InstallationInfo installation1 = installations.First();
            InstallationInfo installation2 = installations.ElementAt(1);
            InstallationInfo installation3 = installations.ElementAt(2);

            this.databaseConnection.DeleteInstallation(installation1.Id);
            this.databaseContext.Installations.Should().BeEquivalentTo(new InstallationInfo[]
                {
                    installation2,
                    installation3,
                });

            this.databaseConnection.DeleteInstallation(installation2.Id);
            this.databaseContext.Installations.Should().BeEquivalentTo(new InstallationInfo[]
                {
                    installation3,
                });

            this.databaseConnection.DeleteInstallation(installation3.Id);
            this.databaseContext.Installations.Should().BeEmpty();
        }

        internal void TestDeleteFileOperations(
            IEnumerable<InstallationInfo> installations,
            IEnumerable<FileChange> fileChanges,
            IEnumerable<FileCreation> fileCreations,
            IEnumerable<FileDeletion> fileDeletions,
            IEnumerable<FileRenaming> fileRenamings)
        {
            InstallationInfo installation1 = installations.First();
            InstallationInfo installation2 = installations.ElementAt(1);
            InstallationInfo installation3 = installations.ElementAt(2);

            FileChange fileChange1 = fileChanges.First();
            FileChange fileChange3 = fileChanges.ElementAt(2);
            FileChange fileChange4 = fileChanges.ElementAt(3);

            FileCreation fileCreation1 = fileCreations.First();
            FileCreation fileCreation3 = fileCreations.ElementAt(2);
            FileCreation fileCreation4 = fileCreations.ElementAt(3);

            FileDeletion fileDeletion1 = fileDeletions.First();
            FileDeletion fileDeletion3 = fileDeletions.ElementAt(2);
            FileDeletion fileDeletion4 = fileDeletions.ElementAt(3);

            FileRenaming fileRenaming1 = fileRenamings.First();
            FileRenaming fileRenaming3 = fileRenamings.ElementAt(2);
            FileRenaming fileRenaming4 = fileRenamings.ElementAt(3);

            this.databaseConnection.DeleteFileOperations(installation2.Id);
            this.databaseContext.FileChanges.Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                    fileChange3,
                    fileChange4,
                });
            this.databaseContext.FileCreations.Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                    fileCreation3,
                    fileCreation4,
                });
            this.databaseContext.FileDeletions.Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                    fileDeletion3,
                    fileDeletion4,
                });
            this.databaseContext.FileRenamings.Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                    fileRenaming3,
                    fileRenaming4,
                });

            this.databaseConnection.DeleteFileOperations(installation3.Id);
            this.databaseContext.FileChanges.Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                });
            this.databaseContext.FileCreations.Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                });
            this.databaseContext.FileDeletions.Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                });
            this.databaseContext.FileRenamings.Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                });

            this.databaseConnection.DeleteFileOperations(installation1.Id);
            this.databaseContext.FileChanges.Should().BeEmpty();
            this.databaseContext.FileCreations.Should().BeEmpty();
            this.databaseContext.FileDeletions.Should().BeEmpty();
            this.databaseContext.FileRenamings.Should().BeEmpty();
        }
    }
}