using FluentAssertions;
using InstallationsMonitor.Commands.Remove.Utilities;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Tests.Utilities;
using InstallationsMonitor.Tests.Utilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.UnitTests.Commands.Remove
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
            using DatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<DatabaseConnection>();
            InstallationsRemover installationsRemover = serviceProvider
                .GetRequiredService<InstallationsRemover>();

            databaseConnection.CreateInstallation(new Installation(programName1, dateTime1));
            databaseConnection.CreateInstallation(new Installation(programName2, dateTime2));
            databaseConnection.CreateInstallation(new Installation(programName3, dateTime3));
            DatabaseChecker.CheckInstallations(
                databaseConnection, new string[] { programName1, programName2, programName3 });

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationsRemover.Remove(installationId);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation with id '{installationId}' removed.{Environment.NewLine}");

            DatabaseChecker.CheckInstallations(
                databaseConnection, new string[] { programName1, programName3 });
        }

        [TestMethod]
        public void Remove_NotExistentIdentifier_RemovesInstallation()
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