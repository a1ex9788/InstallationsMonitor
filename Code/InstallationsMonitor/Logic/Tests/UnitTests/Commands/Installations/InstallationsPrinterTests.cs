using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Logic.Commands.Installations.Utilities;
using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders;
using InstallationsMonitor.Persistence.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.UnitTests.Commands.Installations
{
    [TestClass]
    public class InstallationsPrinterTests
    {
        [TestMethod]
        public void Print_SomeInstallationsExist_PrintsInstallations()
        {
            // Arrange.
            Installation installation1 = new Installation("Program1", new DateTime(1, 1, 1, 1, 1, 1));
            Installation installation2 = new Installation("Program2", new DateTime(2, 2, 2, 2, 2, 2));

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationsCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            InstallationsPrinter installationsPrinter = serviceProvider
                .GetRequiredService<InstallationsPrinter>();

            databaseConnection.CreateInstallation(installation1);
            databaseConnection.CreateInstallation(installation2);
            installation1.Id.Should().Be(1);
            installation2.Id.Should().Be(2);

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