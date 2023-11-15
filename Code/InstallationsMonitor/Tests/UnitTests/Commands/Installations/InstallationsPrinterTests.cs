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
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests.UnitTests.Commands.Installations
{
    [TestClass]
    public class InstallationsPrinterTests
    {
        [TestMethod]
        public async Task ExecuteAsync_ForAllDrives_CreatesInstallationWithAllFiles()
        {
            // Arrange.
            string programName1 = "Program1";
            string programName2 = "Program2";
            DateTime dateTime1 = DateTime.Now;
            DateTime dateTime2 = dateTime1.AddDays(1);

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
@$"{programName1} - {dateTime1}
{programName2} - {dateTime2}
";

            stringWriter.ToString().Should().Be(expectedOutput);
        }
    }
}