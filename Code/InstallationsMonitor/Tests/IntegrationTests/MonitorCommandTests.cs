using FluentAssertions;
using InstallationsMonitor.Commands;
using InstallationsMonitor.Commands.Monitor;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Tests.Utilities;
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
        [TestMethod]
        public async Task MonitorCommand_SomeFilesCreated_PrintsExpectedResults()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            string programName = "Program";
            string[] args = new string[] { "monitor", "-p", programName };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            using DatabaseConnection databaseConnection = DatabaseUtilities.ConfigureTestPersistence();

            CommandsCreator.CreateMonitorCommandImplementation = (directory, programName, _)
                => new MonitorCommand(directory, programName, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => Program.Main(args));

            await EventsUtilities.WaitForEventsRegistrationAsync();

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

            CommandsCreator.CreateMonitorCommandImplementation = (d, _, _) =>
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

            CommandsCreator.CreateMonitorCommandImplementation = (d, _, _) =>
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

            CommandsCreator.CreateMonitorCommandImplementation = (_, pn, _) =>
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

            CommandsCreator.CreateMonitorCommandImplementation = (_, pn, _) =>
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