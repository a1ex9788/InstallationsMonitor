using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using System.Collections.Generic;

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

        internal void TestGetFileChanges(IEnumerable<FileChange> fileChanges)
        {
            this.databaseConnection.GetFileChanges().Should().BeEquivalentTo(fileChanges);
        }

        internal void TestGetFileCreations(IEnumerable<FileCreation> fileCreations)
        {
            this.databaseConnection.GetFileCreations().Should().BeEquivalentTo(fileCreations);
        }

        internal void TestGetFileDeletions(IEnumerable<FileDeletion> fileDeletions)
        {
            this.databaseConnection.GetFileDeletions().Should().BeEquivalentTo(fileDeletions);
        }

        internal void TestGetFileRenamings(IEnumerable<FileRenaming> fileRenamings)
        {
            this.databaseConnection.GetFileRenamings().Should().BeEquivalentTo(fileRenamings);
        }
    }
}