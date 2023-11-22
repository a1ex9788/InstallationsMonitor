using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Logic.Commands.Remove.Utilities;
using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders;
using InstallationsMonitor.Persistence.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.UnitTests.Commands.Remove
{
    [TestClass]
    public class InstallationsRemoverTests
    {
        [TestMethod]
        public void Remove_ExistentIdentifier_RemovesInstallation()
        {
            // Arrange.
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new RemoveCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            InstallationsRemover installationsRemover = serviceProvider
                .GetRequiredService<InstallationsRemover>();

            Installation installation1 = new Installation("Program1", new DateTime(1, 1, 1, 1, 1, 1));
            Installation installation2 = new Installation("Program2", new DateTime(2, 2, 2, 2, 2, 2));
            Installation installation3 = new Installation("Program3", new DateTime(3, 3, 3, 3, 3, 3));

            databaseConnection.CreateInstallation(installation1);
            databaseConnection.CreateInstallation(installation2);
            databaseConnection.CreateInstallation(installation3);
            databaseConnection.GetInstallations().Should().BeEquivalentTo(new Installation[]
                {
                    installation1,
                    installation2,
                    installation3,
                });

            FileChange fileChange1 = new FileChange("File1", DateTime.Now, installation1.Id);
            FileChange fileChange2 = new FileChange("File2", DateTime.Now, installation2.Id);
            FileChange fileChange3 = new FileChange("File3", DateTime.Now, installation3.Id);
            FileCreation fileCreation1 = new FileCreation("File4", DateTime.Now, installation1.Id);
            FileCreation fileCreation2 = new FileCreation("File5", DateTime.Now, installation2.Id);
            FileCreation fileCreation3 = new FileCreation("File6", DateTime.Now, installation3.Id);
            FileDeletion fileDeletion1 = new FileDeletion("File7", DateTime.Now, installation1.Id);
            FileDeletion fileDeletion2 = new FileDeletion("File8", DateTime.Now, installation2.Id);
            FileDeletion fileDeletion3 = new FileDeletion("File9", DateTime.Now, installation3.Id);
            FileRenaming fileRenaming1 = new FileRenaming(
                "File10", DateTime.Now, installation1.Id, "OldFile10");
            FileRenaming fileRenaming2 = new FileRenaming(
                "File11", DateTime.Now, installation2.Id, "OldFile11");
            FileRenaming fileRenaming3 = new FileRenaming(
                "File12", DateTime.Now, installation3.Id, "OldFile12");

            databaseConnection.CreateFileChange(fileChange1);
            databaseConnection.CreateFileChange(fileChange2);
            databaseConnection.CreateFileChange(fileChange3);
            databaseConnection.CreateFileCreation(fileCreation1);
            databaseConnection.CreateFileCreation(fileCreation2);
            databaseConnection.CreateFileCreation(fileCreation3);
            databaseConnection.CreateFileDeletion(fileDeletion1);
            databaseConnection.CreateFileDeletion(fileDeletion2);
            databaseConnection.CreateFileDeletion(fileDeletion3);
            databaseConnection.CreateFileRenaming(fileRenaming1);
            databaseConnection.CreateFileRenaming(fileRenaming2);
            databaseConnection.CreateFileRenaming(fileRenaming3);
            databaseConnection.GetFileChanges().Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                    fileChange2,
                    fileChange3,
                });
            databaseConnection.GetFileCreations().Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                    fileCreation2,
                    fileCreation3,
                });
            databaseConnection.GetFileDeletions().Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                    fileDeletion2,
                    fileDeletion3,
                });
            databaseConnection.GetFileRenamings().Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                    fileRenaming2,
                    fileRenaming3,
                });

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationsRemover.Remove(installation2.Id);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation with id '{installation2.Id}' removed.{Environment.NewLine}");

            databaseConnection.GetInstallations().Should().BeEquivalentTo(new Installation[]
                {
                    installation1,
                    installation3,
                });

            databaseConnection.GetFileChanges().Should().BeEquivalentTo(new FileChange[]
                {
                    fileChange1,
                    fileChange3,
                });
            databaseConnection.GetFileCreations().Should().BeEquivalentTo(new FileCreation[]
                {
                    fileCreation1,
                    fileCreation3,
                });
            databaseConnection.GetFileDeletions().Should().BeEquivalentTo(new FileDeletion[]
                {
                    fileDeletion1,
                    fileDeletion3,
                });
            databaseConnection.GetFileRenamings().Should().BeEquivalentTo(new FileRenaming[]
                {
                    fileRenaming1,
                    fileRenaming3,
                });
        }

        [TestMethod]
        public void Remove_NotExistentIdentifier_PrintsAnyInstallationMessage()
        {
            // Arrange.
            int installationId = 1;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new RemoveCommandTestServiceProvider(
                cancellationTokenSource.Token);
            InstallationsRemover installationsRemover = serviceProvider
                .GetRequiredService<InstallationsRemover>();

            using StringWriter outStringWriter = new StringWriter();
            Console.SetOut(outStringWriter);
            using StringWriter errorStringWriter = new StringWriter();
            Console.SetError(errorStringWriter);

            // Act.
            installationsRemover.Remove(installationId);

            // Assert.
            outStringWriter.ToString().Should().BeEmpty();

            errorStringWriter.ToString().Should().Be(
                $"Any installation with id '{installationId}' exists.{Environment.NewLine}");
        }
    }
}