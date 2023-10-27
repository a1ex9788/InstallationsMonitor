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
        public async Task Execute_FileCreated_PrintsFile()
        {
            // Arrange.
            string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testPath);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(testPath, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => monitorCommand.Execute());

            // Wait for the registrations to the events.
            await Task.Delay(500);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();

            // Assert.
            await TestUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();

            await task;
        }

        [TestMethod]
        public async Task Execute_FileChanged_PrintsFile()
        {
            // Arrange.
            string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testPath);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(testPath, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => monitorCommand.Execute());

            // Wait for the registrations to the events.
            await Task.Delay(500);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.WriteAllText(filePath, string.Empty);

            // Assert.
            await TestUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedChangedFiles: new string[] { filePath },
                expectedCreatedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();

            await task;
        }

        [TestMethod]
        public async Task Execute_FileDeleted_PrintsFile()
        {
            // Arrange.
            string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testPath);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(testPath, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => monitorCommand.Execute());

            // Wait for the registrations to the events.
            await Task.Delay(500);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.Delete(filePath);

            // Assert.
            await TestUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath },
                expectedDeletedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();

            await task;
        }

        [TestMethod]
        public async Task Execute_FileRenamed_PrintsFile()
        {
            // Arrange.
            string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testPath);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(testPath, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => monitorCommand.Execute());

            // Wait for the registrations to the events.
            await Task.Delay(500);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());
            string newFilePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.Move(filePath, newFilePath);

            // Assert.
            await TestUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath },
                expectedRenamedFiles: new (string OldPath, string NewPath)[] { (filePath, newFilePath) });

            cancellationTokenSource.Cancel();

            await task;
        }
    }
}