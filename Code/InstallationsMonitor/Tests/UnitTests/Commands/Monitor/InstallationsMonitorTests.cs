using FluentAssertions;
using InstallationsMonitor.Commands.Monitor.Utilities;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Tests.Utilities;
using InstallationsMonitor.Tests.Utilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using InstallationsMonitorClass = InstallationsMonitor.Commands.Monitor.Utilities.InstallationsMonitor;

namespace InstallationsMonitor.Tests.UnitTests.Commands.Monitor
{
    [TestClass]
    public class InstallationsMonitorTests
    {
        [TestMethod]
        public async Task ExecuteAsync_ForAllDrives_CreatesInstallationWithAllFiles()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            string programName = "Program";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            using DatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<DatabaseConnection>();
            InstallationsMonitorClass installationsMonitor = serviceProvider
                .GetRequiredService<InstallationsMonitorClass>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = installationsMonitor.MonitorAsync(directory: null, programName);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = TempPathsObtainer.GetTempFile();
            string[] filePaths = new string[] { filePath1, filePath2 };

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            foreach (string drive in DrivesObtainer.GetDrives())
            {
                stringWriter.ToString().Should().Contain($"Monitoring directory '{drive}'...");
            }

            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter, expectedCreatedFiles: filePaths);
            cancellationTokenSource.Cancel();
            await task;

            Installation installation = DatabaseChecker.CheckInstallation(
                databaseConnection, programName);
            DatabaseChecker.CheckFileOperations<FileCreation>(
                databaseConnection,
                installation.Id,
                filePaths,
                checkFileOperationsNumber: false);
        }

        [TestMethod]
        public async Task ExecuteAsync_ForConcretePath_CreatesInstallationWithOnlySomeFiles()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            string programName = "Program";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            using DatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<DatabaseConnection>();
            InstallationsMonitorClass installationsMonitor = serviceProvider
                .GetRequiredService<InstallationsMonitorClass>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = installationsMonitor.MonitorAsync(directory: testPath, programName);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = TempPathsObtainer.GetTempFile();

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            stringWriter.ToString().Should().Contain($"Monitoring directory '{testPath}'...");

            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath1 },
                expectedNotCreatedFiles: new string[] { filePath2 });

            cancellationTokenSource.Cancel();
            await task;

            Installation installation = DatabaseChecker.CheckInstallation(
                databaseConnection, programName);
            DatabaseChecker.CheckFileOperations<FileCreation>(
                databaseConnection, installation.Id, new string[] { filePath1 });
        }

        [TestMethod]
        public async Task ExecuteAsync_ProgramNameNotSpecified_ProgramNameFromConsoleReadLine()
        {
            // Arrange.
            string programName = "Program";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            using DatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<DatabaseConnection>();
            InstallationsMonitorClass installationsMonitor = serviceProvider
                .GetRequiredService<InstallationsMonitorClass>();

            using StringReader stringReader = new StringReader(programName);
            Console.SetIn(stringReader);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = installationsMonitor.MonitorAsync(directory: null, programName);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            cancellationTokenSource.Cancel();
            await task;

            // Assert.
            stringWriter.ToString().Should().Contain(
                $"Monitoring installation of program '{programName}'...");

            DatabaseChecker.CheckInstallation(databaseConnection, programName);
        }

        [TestMethod]
        public async Task ExecuteAsync_ProgramNameSpecified_ProgramNameFromArguments()
        {
            // Arrange.
            string programName = "Program";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            using DatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<DatabaseConnection>();
            InstallationsMonitorClass installationsMonitor = serviceProvider
                .GetRequiredService<InstallationsMonitorClass>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = installationsMonitor.MonitorAsync(directory: null, programName);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            cancellationTokenSource.Cancel();
            await task;

            // Assert.
            stringWriter.ToString().Should().Contain(
                $"Monitoring installation of program '{programName}'...");

            DatabaseChecker.CheckInstallation(databaseConnection, programName);
        }

        [TestMethod]
        public async Task ExecuteAsync_RepeatedProgramName_CreatesInstallationWithSameProgramName()
        {
            // Arrange.
            string programName = "Program";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            using DatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<DatabaseConnection>();
            InstallationsMonitorClass installationsMonitor = serviceProvider
                .GetRequiredService<InstallationsMonitorClass>();

            databaseConnection.CreateInstallation(new Installation(programName, DateTime.Now));
            DatabaseChecker.CheckInstallation(databaseConnection, programName);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = installationsMonitor.MonitorAsync(directory: null, programName);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            cancellationTokenSource.Cancel();
            await task;

            // Assert.
            stringWriter.ToString().Should().Contain(
                $"Monitoring installation of program '{programName}'...");

            DatabaseChecker.CheckInstallations(
                databaseConnection, new string[] { programName, programName });
        }
    }
}