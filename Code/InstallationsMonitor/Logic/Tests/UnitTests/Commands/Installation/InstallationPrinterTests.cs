using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Logic.Commands.Installation.Utilities;
using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders;
using InstallationsMonitor.Persistence.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.UnitTests.Commands.Installation
{
    [TestClass]
    public class InstallationPrinterTests
    {
        [TestMethod]
        public void Print_InstallationExists_PrintsInstallation()
        {
            // Arrange.
            InstallationInfo installation = new InstallationInfo("Program", DateTime.Now);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            InstallationPrinter installationPrinter = serviceProvider
                .GetRequiredService<InstallationPrinter>();

            databaseConnection.CreateInstallation(installation);

            FileChange fileChange = new FileChange(
                "FileChange", new DateTime(1, 1, 1, 1, 1, 1), installation.Id);
            databaseConnection.CreateFileChange(fileChange);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationPrinter.Print(installation.Id);

            // Assert.
            stringWriter.ToString().Should().Be(
                Environment.NewLine +
                $"File changes:{Environment.NewLine}" +
                $"|   FilePath |                Date |{Environment.NewLine}" +
                $"------------------------------------{Environment.NewLine}" +
                $"| FileChange | 01/01/0001 01:01:01 |{Environment.NewLine}");
        }

        [TestMethod]
        public void Print_InstallationWithoutFileOperations_PrintsInstallationDoesNotHaveFileOperationsMessage()
        {
            // Arrange.
            InstallationInfo installation = new InstallationInfo("Program1", new DateTime(1, 1, 1, 1, 1, 1));

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            InstallationPrinter installationPrinter = serviceProvider
                .GetRequiredService<InstallationPrinter>();

            databaseConnection.CreateInstallation(installation);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationPrinter.Print(installation.Id);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation '{installation.Id}' has not any file operation.{Environment.NewLine}");
        }

        [TestMethod]
        public void Print_InstallationDoesNotExist_PrintsInstallationDoesNotExistMessage()
        {
            // Arrange.
            int installationId = 1;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationCommandTestServiceProvider(
                cancellationTokenSource.Token);
            InstallationPrinter installationPrinter = serviceProvider
                .GetRequiredService<InstallationPrinter>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationPrinter.Print(installationId);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation '{installationId}' does not exist.{Environment.NewLine}");
        }
    }
}