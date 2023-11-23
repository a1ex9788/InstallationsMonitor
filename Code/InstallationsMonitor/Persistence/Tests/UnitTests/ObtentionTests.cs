using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Tests.UnitTests
{
    internal class ObtentionTests
    {
        private readonly IDatabaseConnection databaseConnection;

        internal ObtentionTests(IDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        internal void TestGetInstallations(IEnumerable<InstallationInfo> installations)
        {
            this.databaseConnection.GetInstallations().Should().BeEquivalentTo(installations);
        }

        internal void TestGetInstallation(IEnumerable<InstallationInfo> installations)
        {
            foreach (InstallationInfo installation in installations)
            {
                this.databaseConnection.GetInstallation(installation.Id).Should().Be(installation);
            }
        }

        internal void TestGetFileChanges(
            IEnumerable<InstallationInfo> installations, IEnumerable<FileChange> fileChanges)
        {
            this.databaseConnection.GetFileChanges().Should().BeEquivalentTo(fileChanges);

            InstallationInfo installation3 = installations.ElementAt(2);
            FileChange fileChange3 = fileChanges.ElementAt(2);
            FileChange fileChange4 = fileChanges.ElementAt(3);
            this.databaseConnection.GetFileChanges(installation3.Id)
                .Should().BeEquivalentTo(new FileChange[]
                    {
                        fileChange3,
                        fileChange4,
                    });
        }

        internal void TestGetFileCreations(
            IEnumerable<InstallationInfo> installations, IEnumerable<FileCreation> fileCreations)
        {
            this.databaseConnection.GetFileCreations().Should().BeEquivalentTo(fileCreations);

            InstallationInfo installation3 = installations.ElementAt(2);
            FileCreation fileCreation3 = fileCreations.ElementAt(2);
            FileCreation fileCreation4 = fileCreations.ElementAt(3);
            this.databaseConnection.GetFileCreations(installation3.Id)
                .Should().BeEquivalentTo(new FileCreation[]
                    {
                        fileCreation3,
                        fileCreation4,
                    });
        }

        internal void TestGetFileDeletions(
            IEnumerable<InstallationInfo> installations, IEnumerable<FileDeletion> fileDeletions)
        {
            this.databaseConnection.GetFileDeletions().Should().BeEquivalentTo(fileDeletions);

            InstallationInfo installation3 = installations.ElementAt(2);
            FileDeletion fileDeletion3 = fileDeletions.ElementAt(2);
            FileDeletion fileDeletion4 = fileDeletions.ElementAt(3);
            this.databaseConnection.GetFileDeletions(installation3.Id)
                .Should().BeEquivalentTo(new FileDeletion[]
                    {
                        fileDeletion3,
                        fileDeletion4,
                    });
        }

        internal void TestGetFileRenamings(
            IEnumerable<InstallationInfo> installations, IEnumerable<FileRenaming> fileRenamings)
        {
            this.databaseConnection.GetFileRenamings().Should().BeEquivalentTo(fileRenamings);

            InstallationInfo installation3 = installations.ElementAt(2);
            FileRenaming fileRenaming3 = fileRenamings.ElementAt(2);
            FileRenaming fileRenaming4 = fileRenamings.ElementAt(3);
            this.databaseConnection.GetFileRenamings(installation3.Id)
                .Should().BeEquivalentTo(new FileRenaming[]
                    {
                        fileRenaming3,
                        fileRenaming4,
                    });
        }
    }
}