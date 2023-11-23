using FluentAssertions;
using InstallationsMonitor;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.ServiceProviders.Base;
using InstallationsMonitor.Tests.Utilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationMonitor.Tests.IntegrationTests.Commands
{
    [TestClass]
    public class InstallationCommandTests
    {
        [TestMethod]
        public void InstallationCommand_SomeInstallationExist_PrintsExpectedResults()
        {
            // Arrange.
            InstallationInfo installation = new InstallationInfo("Program", new DateTime(1, 1, 1, 1, 1, 1))
            {
                Id = 1,
            };

            string[] args = new string[] { "installation", "-i", "1" };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationCommandTestServiceProvider(
                cancellationTokenSource.Token);
            DatabaseContext databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            databaseContext.Installations.Add(installation);
            databaseContext.SaveChanges();

            CommandsServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(serviceProvider.GetRequiredService<DatabaseOptions>());

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation '{installation.Id}' has not any file operation.{Environment.NewLine}");
        }

        [TestMethod]
        public void InstallationCommand_WithoutIdentifier_ThrowsException()
        {
            // Arrange.
            string[] args = new string[] { "installation" };

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
        public void InstallationCommand_NotIntegerIdentifier_ThrowsException()
        {
            // Arrange.
            string[] args = new string[] { "installation", "-i", "notInteger" };

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