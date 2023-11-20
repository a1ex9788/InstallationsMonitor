using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Persistence.Contracts;
using InstallationsMonitor.ServiceProviders.Base;
using InstallationsMonitor.TestsUtilities;
using InstallationsMonitor.TestsUtilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.IntegrationTests.Commands
{
    [TestClass]
    public class RemoveInstallationCommandTests
    {
        [TestMethod]
        public void RemoveCommand_ExistentIdentifier_RemovesInstallation()
        {
            // Arrange.
            string programName = "Program";
            DateTime dateTime = new DateTime(1, 1, 1, 1, 1, 1);

            string installationId = "1";
            string[] args = new string[] { "remove", "-i", installationId };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new RemoveCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();

            databaseConnection.CreateInstallation(new Installation(programName, dateTime));
            DatabaseChecker.CheckInstallation(databaseConnection, programName);

            CommandsServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(serviceProvider.GetRequiredService<DatabaseOptions>());

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation with id '{installationId}' removed.{Environment.NewLine}");

            DatabaseChecker.CheckInstallations(databaseConnection, Array.Empty<string>());
        }

        [TestMethod]
        public void RemoveCommand_WithoutIdentifier_ThrowsException()
        {
            // Arrange.
            string[] args = new string[] { "remove" };

            using StringWriter outStringWriter = new StringWriter();
            Console.SetOut(outStringWriter);

            using StringWriter errorStringWriter = new StringWriter();
            Console.SetError(errorStringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            outStringWriter.ToString().Should().BeEmpty();

            errorStringWriter.ToString().Should().Contain(
                $"The -i field is required.{Environment.NewLine}");
        }

        [TestMethod]
        public void RemoveCommand_NotIntegerIdentifier_ThrowsException()
        {
            // Arrange.
            string[] args = new string[] { "remove", "-i", "notInteger" };

            using StringWriter outStringWriter = new StringWriter();
            Console.SetOut(outStringWriter);

            using StringWriter errorStringWriter = new StringWriter();
            Console.SetError(errorStringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            outStringWriter.ToString().Should().BeEmpty();

            errorStringWriter.ToString().Should().Contain("The -i field must be an integer.");
        }
    }
}