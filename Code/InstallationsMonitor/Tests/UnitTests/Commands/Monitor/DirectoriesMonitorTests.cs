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
    public class DirectoriesMonitorTests
    {
        [TestMethod]
        public async Task MonitorAsync_FileChanged_PrintsFile()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = DirectoriesMonitor.MonitorAsync(testPath, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync();

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.WriteAllText(filePath, string.Empty);

            // Assert.
            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedChangedFiles: new string[] { filePath },
                expectedCreatedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();
            await task;
        }

        [TestMethod]
        public async Task MonitorAsync_FileCreated_PrintsFile()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = DirectoriesMonitor.MonitorAsync(testPath, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync();

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();

            // Assert.
            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();
            await task;
        }

        [TestMethod]
        public async Task MonitorAsync_FileDeleted_PrintsFile()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = DirectoriesMonitor.MonitorAsync(testPath, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync();

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.Delete(filePath);

            // Assert.

            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath },
                expectedDeletedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();
            await task;
        }

        [TestMethod]
        public async Task MonitorAsync_FileRenamed_PrintsFile()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = DirectoriesMonitor.MonitorAsync(testPath, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync();

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());
            string newFilePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.Move(filePath, newFilePath);

            // Assert.
            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath },
                expectedRenamedFiles: new (string OldPath, string NewPath)[] { (filePath, newFilePath) });

            cancellationTokenSource.Cancel();
            await task;
        }
    }
}