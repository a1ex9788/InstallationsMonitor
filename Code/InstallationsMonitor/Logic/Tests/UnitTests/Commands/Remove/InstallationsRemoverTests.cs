using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Logic.Commands.Remove.Utilities;
using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders;
using InstallationsMonitor.Persistence.Contracts;
using InstallationsMonitor.TestsUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
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
            string programName1 = "Program1";
            string programName2 = "Program2";
            string programName3 = "Program3";
            DateTime dateTime1 = new DateTime(1, 1, 1, 1, 1, 1);
            DateTime dateTime2 = new DateTime(2, 2, 2, 2, 2, 2);
            DateTime dateTime3 = new DateTime(3, 3, 3, 3, 3, 3);
            int installationId = 2;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new RemoveCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            InstallationsRemover installationsRemover = serviceProvider
                .GetRequiredService<InstallationsRemover>();

            databaseConnection.CreateInstallation(new Installation(programName1, dateTime1));
            databaseConnection.CreateInstallation(new Installation(programName2, dateTime2));
            databaseConnection.CreateInstallation(new Installation(programName3, dateTime3));
            DatabaseChecker.CheckInstallations(
                databaseConnection, new string[] { programName1, programName2, programName3 });

            databaseConnection.CreateFileChange(new FileChange("File1", DateTime.Now, 1));
            databaseConnection.CreateFileCreation(new FileCreation("File2", DateTime.Now, 1));
            databaseConnection.CreateFileDeletion(new FileDeletion("File3", DateTime.Now, 1));
            databaseConnection.CreateFileRenaming(new FileRenaming("File4", DateTime.Now, 1, "OldFile4"));
            databaseConnection.CreateFileChange(new FileChange("File5", DateTime.Now, 2));
            databaseConnection.CreateFileCreation(new FileCreation("File6", DateTime.Now, 2));
            databaseConnection.CreateFileDeletion(new FileDeletion("File7", DateTime.Now, 2));
            databaseConnection.CreateFileRenaming(new FileRenaming("File8", DateTime.Now, 2, "OldFile8"));
            databaseConnection.CreateFileChange(new FileChange("File9", DateTime.Now, 3));
            databaseConnection.CreateFileCreation(new FileCreation("File10", DateTime.Now, 3));
            databaseConnection.CreateFileDeletion(new FileDeletion("File11", DateTime.Now, 3));
            databaseConnection.CreateFileRenaming(new FileRenaming("File12", DateTime.Now, 3, "OldFile12"));

            databaseConnection.GetFileChanges().Should().HaveCount(3);
            databaseConnection.GetFileCreations().Should().HaveCount(3);
            databaseConnection.GetFileDeletions().Should().HaveCount(3);
            databaseConnection.GetFileRenamings().Should().HaveCount(3);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationsRemover.Remove(installationId);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation with id '{installationId}' removed.{Environment.NewLine}");

            DatabaseChecker.CheckInstallations(
                databaseConnection, new string[] { programName1, programName3 });

            databaseConnection.GetFileChanges().Should().HaveCount(2);
            databaseConnection.GetFileCreations().Should().HaveCount(2);
            databaseConnection.GetFileDeletions().Should().HaveCount(2);
            databaseConnection.GetFileRenamings().Should().HaveCount(2);

            databaseConnection.GetFileChanges().Where(fc => fc.InstallationId == installationId)
                .Should().HaveCount(0);
            databaseConnection.GetFileCreations().Where(fc => fc.InstallationId == installationId)
                .Should().HaveCount(0);
            databaseConnection.GetFileDeletions().Where(fd => fd.InstallationId == installationId)
                .Should().HaveCount(0);
            databaseConnection.GetFileRenamings().Where(fr => fr.InstallationId == installationId)
                .Should().HaveCount(0);
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