using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Tests.UnitTests
{
    internal class CreationTests
    {
        private readonly IDatabaseConnection databaseConnection;
        private readonly AppDbContext appDbContext;

        internal CreationTests(IDatabaseConnection databaseConnection, AppDbContext appDbContext)
        {
            this.databaseConnection = databaseConnection;
            this.appDbContext = appDbContext;
        }

        internal IEnumerable<Installation> TestCreateInstallation()
        {
            Installation installation1 = new Installation("Program1", DateTime.MinValue.AddDays(1));
            this.databaseConnection.CreateInstallation(installation1);
            this.appDbContext.Installations.Should().BeEquivalentTo(new Installation[]
                {
                    installation1,
                });

            Installation installation2 = new Installation("Program2", DateTime.MinValue.AddDays(2));
            this.databaseConnection.CreateInstallation(installation2);
            this.appDbContext.Installations.Should().BeEquivalentTo(new Installation[]
                {
                    installation1,
                    installation2,
                });

            Installation installation3 = new Installation("Program3", DateTime.MinValue.AddDays(3));
            this.databaseConnection.CreateInstallation(installation3);
            this.appDbContext.Installations.Should().BeEquivalentTo(new Installation[]
                {
                    installation1,
                    installation2,
                    installation3,
                });

            return new Installation[]
            {
                installation1,
                installation2,
                installation3,
            };
        }

        internal IEnumerable<FileChange> TestCreateFileChanges(IEnumerable<Installation> installations)
        {
            Installation installation1 = installations.First();
            FileChange fileChange1 = new FileChange(
                "FileChange1", DateTime.MinValue.AddMinutes(1), installation1.Id);
            this.databaseConnection.CreateFileChange(fileChange1);
            this.appDbContext.FileChanges.Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                });

            Installation installation2 = installations.ElementAt(1);
            FileChange fileChange2 = new FileChange(
                "FileChange2", DateTime.MinValue.AddMinutes(2), installation2.Id);
            this.databaseConnection.CreateFileChange(fileChange2);
            this.appDbContext.FileChanges.Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                    fileChange2,
                });

            Installation installation3 = installations.ElementAt(2);
            FileChange fileChange3 = new FileChange(
                "FileChange3", DateTime.MinValue.AddMinutes(3), installation3.Id);
            this.databaseConnection.CreateFileChange(fileChange3);
            this.appDbContext.FileChanges.Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                    fileChange2,
                    fileChange3,
                });

            FileChange fileChange4 = new FileChange(
                "FileChange4", DateTime.MinValue.AddMinutes(4), installation3.Id);
            this.databaseConnection.CreateFileChange(fileChange4);
            this.appDbContext.FileChanges.Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                    fileChange2,
                    fileChange3,
                    fileChange4,
                });

            return new FileChange[]
            {
                fileChange1,
                fileChange2,
                fileChange3,
                fileChange4,
            };
        }

        internal IEnumerable<FileCreation> TestCreateFileCreations(IEnumerable<Installation> installations)
        {
            Installation installation1 = this.appDbContext.Installations.First();
            FileCreation fileCreation1 = new FileCreation(
                "FileCreation1", DateTime.MinValue.AddMinutes(1), installation1.Id);
            this.databaseConnection.CreateFileCreation(fileCreation1);
            this.appDbContext.FileCreations.Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                });

            Installation installation2 = installations.ElementAt(1);
            FileCreation fileCreation2 = new FileCreation(
                "FileCreation2", DateTime.MinValue.AddMinutes(2), installation2.Id);
            this.databaseConnection.CreateFileCreation(fileCreation2);
            this.appDbContext.FileCreations.Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                    fileCreation2,
                });

            Installation installation3 = installations.ElementAt(2);
            FileCreation fileCreation3 = new FileCreation(
                "FileCreation3", DateTime.MinValue.AddMinutes(3), installation3.Id);
            this.databaseConnection.CreateFileCreation(fileCreation3);
            this.appDbContext.FileCreations.Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                    fileCreation2,
                    fileCreation3,
                });

            FileCreation fileCreation4 = new FileCreation(
                "FileCreation4", DateTime.MinValue.AddMinutes(4), installation3.Id);
            this.databaseConnection.CreateFileCreation(fileCreation4);
            this.appDbContext.FileCreations.Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                    fileCreation2,
                    fileCreation3,
                    fileCreation4,
                });

            return new FileCreation[]
            {
                fileCreation1,
                fileCreation2,
                fileCreation3,
                fileCreation4,
            };
        }

        internal IEnumerable<FileDeletion> TestCreateFileDeletions(IEnumerable<Installation> installations)
        {
            Installation installation1 = installations.First();
            FileDeletion fileDeletion1 = new FileDeletion(
                "FileDeletion1", DateTime.MinValue.AddMinutes(1), installation1.Id);
            this.databaseConnection.CreateFileDeletion(fileDeletion1);
            this.appDbContext.FileDeletions.Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                });

            Installation installation2 = installations.ElementAt(1);
            FileDeletion fileDeletion2 = new FileDeletion(
                "FileDeletion2", DateTime.MinValue.AddMinutes(2), installation2.Id);
            this.databaseConnection.CreateFileDeletion(fileDeletion2);
            this.appDbContext.FileDeletions.Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                    fileDeletion2,
                });

            Installation installation3 = installations.ElementAt(2);
            FileDeletion fileDeletion3 = new FileDeletion(
                "FileDeletion3", DateTime.MinValue.AddMinutes(3), installation3.Id);
            this.databaseConnection.CreateFileDeletion(fileDeletion3);
            this.appDbContext.FileDeletions.Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                    fileDeletion2,
                    fileDeletion3,
                });

            FileDeletion fileDeletion4 = new FileDeletion(
                "FileDeletion4", DateTime.MinValue.AddMinutes(4), installation3.Id);
            this.databaseConnection.CreateFileDeletion(fileDeletion4);
            this.appDbContext.FileDeletions.Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                    fileDeletion2,
                    fileDeletion3,
                    fileDeletion4,
                });

            return new FileDeletion[]
            {
                fileDeletion1,
                fileDeletion2,
                fileDeletion3,
                fileDeletion4,
            };
        }

        internal IEnumerable<FileRenaming> TestCreateFileRenamings(IEnumerable<Installation> installations)
        {
            Installation installation1 = installations.First();
            FileRenaming fileRenaming1 = new FileRenaming(
                "FileRenaming1", DateTime.MinValue.AddMinutes(1), installation1.Id, "OldFile1");
            this.databaseConnection.CreateFileRenaming(fileRenaming1);
            this.appDbContext.FileRenamings.Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                });

            Installation installation2 = installations.ElementAt(1);
            FileRenaming fileRenaming2 = new FileRenaming(
                "FileRenaming2", DateTime.MinValue.AddMinutes(2), installation2.Id, "OldFile2");
            this.databaseConnection.CreateFileRenaming(fileRenaming2);
            this.appDbContext.FileRenamings.Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                    fileRenaming2,
                });

            Installation installation3 = installations.ElementAt(2);
            FileRenaming fileRenaming3 = new FileRenaming(
                "FileRenaming3", DateTime.MinValue.AddMinutes(3), installation3.Id, "OldFile3");
            this.databaseConnection.CreateFileRenaming(fileRenaming3);
            this.appDbContext.FileRenamings.Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                    fileRenaming2,
                    fileRenaming3,
                });

            FileRenaming fileRenaming4 = new FileRenaming(
                "FileRenaming4", DateTime.MinValue.AddMinutes(4), installation3.Id, "OldFile4");
            this.databaseConnection.CreateFileRenaming(fileRenaming4);
            this.appDbContext.FileRenamings.Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                    fileRenaming2,
                    fileRenaming3,
                    fileRenaming4,
                });

            return new FileRenaming[]
            {
                fileRenaming1,
                fileRenaming2,
                fileRenaming3,
                fileRenaming4,
            };
        }
    }
}