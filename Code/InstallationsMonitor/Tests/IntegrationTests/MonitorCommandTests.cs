using FluentAssertions;
using InstallationsMonitor.Commands;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests.IntegrationTests
{
    [TestClass]
    public class MonitorCommandTests
    {
        private readonly Func<IServiceProvider, string?, string?, ICommand>
            CreateMonitorCommandFunction = CommandsCreator.CreateMonitorCommandFunction;

        [TestCleanup]
        public void TestCleanUp()
        {
            CommandsCreator.CreateMonitorCommandFunction = this.CreateMonitorCommandFunction;
        }

        [TestMethod]
        public async Task MonitorCommand_SomeFilesCreated_PrintsExpectedResults()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            string programName = "Program";
            string[] args = new string[] { "monitor", "-p", programName };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            using DatabaseConnection databaseConnection = DatabaseUtilities.GetTestDatabaseConnection();

            CommandsCreator.ExtraRegistrationsAction =
                sc =>
                {
                    sc.AddSingleton(typeof(CancellationToken), cancellationTokenSource.Token);
                    sc.AddSingleton(databaseConnection);
                };

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => Program.Main(args));

            await EventsUtilities.WaitForEventsRegistrationAsync(stringWriter);

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = TempPathUtilities.GetTempFile();
            string[] filePaths = new string[] { filePath1, filePath2 };

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: filePaths);

            cancellationTokenSource.Cancel();
            await task;

            // Add checks.
            Installation installation = DatabaseUtilities.CheckInstallation(
                databaseConnection, programName);
            DatabaseUtilities.CheckFileOperations<FileCreation>(
                databaseConnection,
                installation.Id,
                filePaths,
                checkFileOperationsNumber: false);
        }

        [TestMethod]
        public void MonitorCommand_DirectoryNotSpecified_NullStringPassed()
        {
            // Arrange.
            string[] args = new string[] { "monitor" };

            string? directoryPassed = null;

            CommandsCreator.CreateMonitorCommandFunction = (_, d, _) =>
            {
                directoryPassed = d;

                return new Mock<ICommand>().Object;
            };

            // Act.
            Program.Main(args);

            // Assert.
            directoryPassed.Should().Be(null);
        }

        [TestMethod]
        public void MonitorCommand_DirectorySpecified_DirectoryPassed()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            string[] args = new string[] { "monitor", "-d", testPath };

            string? directoryPassed = null;

            CommandsCreator.CreateMonitorCommandFunction = (_, d, _) =>
            {
                directoryPassed = d;

                return new Mock<ICommand>().Object;
            };

            // Act.
            Program.Main(args);

            // Assert.
            directoryPassed.Should().Be(testPath);
        }

        [TestMethod]
        public void MonitorCommand_ProgramNameNotSpecified_NullStringPassed()
        {
            // Arrange.
            string[] args = new string[] { "monitor" };

            string? programNamePassed = null;

            CommandsCreator.CreateMonitorCommandFunction = (_, _, pn) =>
            {
                programNamePassed = pn;

                return new Mock<ICommand>().Object;
            };

            // Act.
            Program.Main(args);

            // Assert.
            programNamePassed.Should().Be(null);
        }

        [TestMethod]
        public void MonitorCommand_ProgramNameSpecified_ProgramNamePassed()
        {
            // Arrange.
            string programName = "Program";
            string[] args = new string[] { "monitor", "-p", programName };

            string? programNamePassed = null;

            CommandsCreator.CreateMonitorCommandFunction = (_, _, pn) =>
            {
                programNamePassed = pn;

                return new Mock<ICommand>().Object;
            };

            // Act.
            Program.Main(args);

            // Assert.
            programNamePassed.Should().Be(programName);
        }
    }
}