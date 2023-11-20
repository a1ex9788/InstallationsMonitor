using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Persistence.Contracts;
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
            string programName = "Program";
            DateTime dateTime = new DateTime(1, 1, 1, 1, 1, 1);

            string[] args = new string[] { "installations", };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new InstallationsCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();

            databaseConnection.CreateInstallation(new Installation(programName, dateTime));

            CommandsServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(serviceProvider.GetRequiredService<DatabaseOptions>());

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            string expectedOutput =
                $"| Id | Program name |                Date |{Environment.NewLine}" +
                $"-------------------------------------------{Environment.NewLine}" +
                $"|  1 |      Program | 01/01/0001 01:01:01 |{Environment.NewLine}";

            stringWriter.ToString().Should().Be(expectedOutput);
        }
    }
}