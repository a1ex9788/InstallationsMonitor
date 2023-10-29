using InstallationsMonitor.Commands.Monitor;
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

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(
                directory: null, cancellationTokenSource.Token);

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

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(testPath, cancellationTokenSource.Token);

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
            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath1 },
                expectedNotCreatedFiles: new string[] { filePath2 });

            cancellationTokenSource.Cancel();

            await task;
        }
    }
}