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

        internal void TestDeleteInstallation(IEnumerable<Installation> installations)
        {
            Installation installation1 = installations.First();
            Installation installation2 = installations.ElementAt(1);
            Installation installation3 = installations.ElementAt(2);

            this.databaseConnection.RemoveInstallation(installation1.Id);
            this.databaseContext.Installations.Should().BeEquivalentTo(new Installation[]
                {
                    installation2,
                    installation3,
                });

            this.databaseConnection.RemoveInstallation(installation2.Id);
            this.databaseContext.Installations.Should().BeEquivalentTo(new Installation[]
                {
                    installation3,
                });

            this.databaseConnection.RemoveInstallation(installation3.Id);
            this.databaseContext.Installations.Should().BeEmpty();
        }

        internal void TestDeleteFileOperations(
            IEnumerable<Installation> installations,
            IEnumerable<FileChange> fileChanges,
            IEnumerable<FileCreation> fileCreations,
            IEnumerable<FileDeletion> fileDeletions,
            IEnumerable<FileRenaming> fileRenamings)
        {
            Installation installation1 = installations.First();
            Installation installation2 = installations.ElementAt(1);
            Installation installation3 = installations.ElementAt(2);

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

            this.databaseConnection.RemoveFileOperations(installation2.Id);
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

            this.databaseConnection.RemoveFileOperations(installation3.Id);
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

            this.databaseConnection.RemoveFileOperations(installation1.Id);
            this.databaseContext.FileChanges.Should().BeEmpty();
            this.databaseContext.FileCreations.Should().BeEmpty();
            this.databaseContext.FileDeletions.Should().BeEmpty();
            this.databaseContext.FileRenamings.Should().BeEmpty();
        }
    }
}