using FluentAssertions;
using InstallationsMonitor.Commands.Installations.Utilities;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Tests.Utilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.UnitTests.Commands.Installations
{
    [TestClass]
    public class InstallationsPrinterTests
    {
        [TestMethod]
        public void Print_SomeInstallationsExist_PrintsInstallations()
        {
            // Arrange.
            string programName1 = "Program1";
            string programName2 = "Program2";
            DateTime dateTime1 = new DateTime(1, 1, 1, 1, 1, 1);
            DateTime dateTime2 = new DateTime(2, 2, 2, 2, 2, 2);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationsCommandTestServiceProvider(
                cancellationTokenSource.Token);
            using DatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<DatabaseConnection>();
            InstallationsPrinter installationsPrinter = serviceProvider
                .GetRequiredService<InstallationsPrinter>();

            databaseConnection.CreateInstallation(new Installation(programName1, dateTime1));
            databaseConnection.CreateInstallation(new Installation(programName2, dateTime2));

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationsPrinter.Print();

            // Assert.
            string expectedOutput =
                $"| Id | Program name |                Date |{Environment.NewLine}" +
                $"-------------------------------------------{Environment.NewLine}" +
                $"|  1 |     Program1 | 01/01/0001 01:01:01 |{Environment.NewLine}" +
                $"|  2 |     Program2 | 02/02/0002 02:02:02 |{Environment.NewLine}";

            stringWriter.ToString().Should().Be(expectedOutput);
        }

        [TestMethod]
        public void Print_NoInstallationsExist_PrintsNoInstallationsMessage()
        {
            // Arrange.
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationsCommandTestServiceProvider(
                cancellationTokenSource.Token);
            InstallationsPrinter installationsPrinter = serviceProvider
                .GetRequiredService<InstallationsPrinter>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            installationsPrinter.Print();

            // Assert.
            string expectedOutput = $"No installations monitored yet.{Environment.NewLine}";

            stringWriter.ToString().Should().Be(expectedOutput);
        }
    }
}