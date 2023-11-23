using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.ServiceProviders.Base;
using InstallationsMonitor.Tests.Utilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.IntegrationTests.Commands
{
    [TestClass]
    public class InstallationsCommandTests
    {
        [TestMethod]
        public void InstallationsCommand_SomeInstallationsExist_PrintsExpectedResults()
        {
            // Arrange.
            InstallationInfo installation = new InstallationInfo("Program", new DateTime(1, 1, 1, 1, 1, 1))
            {
                Id = 1,
            };

            string[] args = new string[] { "installations", };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationsCommandTestServiceProvider(
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
                $"| Id | Program name |                Date |{Environment.NewLine}" +
                $"-------------------------------------------{Environment.NewLine}" +
                $"|  1 |      Program | 01/01/0001 01:01:01 |{Environment.NewLine}");
        }
    }
}