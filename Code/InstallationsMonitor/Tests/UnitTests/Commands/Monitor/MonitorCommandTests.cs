using FluentAssertions;
using InstallationsMonitor.Commands.Monitor;
using InstallationsMonitor.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests.UnitTests.Commands.Monitor
{
    [TestClass]
    public class MonitorCommandTests
    {
        [TestMethod]
        public async Task ExecuteAsync_ForAllDrives_PrintsAllFiles()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            string programName = "Program";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(
                directory: null, programName, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = monitorCommand.ExecuteAsync();

            await EventsUtilities.WaitForEventsRegistrationAsync();

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = TempPathUtilities.GetTempFile();

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            foreach (string drive in DrivesObtainer.GetDrives())
            {
                stringWriter.ToString().Should().Contain($"Monitoring directory '{drive}'...");
            }

            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath1, filePath2 });

            cancellationTokenSource.Cancel();
            await task;
        }

        [TestMethod]
        public async Task ExecuteAsync_ForConcretePath_PrintsOnlySomeFiles()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            string programName = "Program";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(
                testPath, programName, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = monitorCommand.ExecuteAsync();

            await EventsUtilities.WaitForEventsRegistrationAsync();

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = TempPathUtilities.GetTempFile();

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            stringWriter.ToString().Should().Contain($"Monitoring directory '{testPath}'...");

            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath1 },
                expectedNotCreatedFiles: new string[] { filePath2 });

            cancellationTokenSource.Cancel();
            await task;
        }

        [TestMethod]
        public async Task ExecuteAsync_ProgramNameNotSpecified_ProgramNameFromConsoleReadLine()
        {
            // Arrange.
            string programName = "Program";
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(
                directory: null, programName: null, cancellationTokenSource.Token);

            using StringReader stringReader = new StringReader(programName);
            Console.SetIn(stringReader);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = monitorCommand.ExecuteAsync();

            await EventsUtilities.WaitForEventsRegistrationAsync();

            cancellationTokenSource.Cancel();
            await task;

            // Assert.
            stringWriter.ToString().Should().Contain(
                $"Monitoring installation of program '{programName}'...");
        }

        [TestMethod]
        public async Task ExecuteAsync_ProgramNameSpecified_ProgramNameFromArguments()
        {
            // Arrange.
            string programName = "Program";
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(
                directory: null, programName: programName, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = monitorCommand.ExecuteAsync();

            await EventsUtilities.WaitForEventsRegistrationAsync();

            cancellationTokenSource.Cancel();
            await task;

            // Assert.
            stringWriter.ToString().Should().Contain(
                $"Monitoring installation of program '{programName}'...");
        }
    }
}