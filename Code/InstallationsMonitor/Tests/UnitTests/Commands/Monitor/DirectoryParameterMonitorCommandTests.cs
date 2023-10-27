using InstallationsMonitor.Commands.Monitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests.UnitTests.Commands.Monitor
{
    [TestClass]
    public class DirectoryParameterMonitorCommandTests
    {
        [TestMethod]
        public async Task ExecuteForAllDrives_SomeFilesCreatedInDifferentFolders_PrintsAllFiles()
        {
            // Arrange.
            string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testPath);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            MonitorCommand monitorCommand = new MonitorCommand(
                directory: null, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => monitorCommand.Execute());

            // Wait for the registrations to the events.
            await Task.Delay(500);

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = Path.Combine(Path.GetTempFileName());

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            await TestUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath1, filePath2 });

            cancellationTokenSource.Cancel();

            await task;
        }

        [TestMethod]
        public async Task ExecuteForConcretePath_SomeFilesCreatedInDifferentFolders_PrintsOnlySomeFiles()
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

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = Path.Combine(Path.GetTempFileName());

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            await TestUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath1 },
                expectedNotCreatedFiles: new string[] { filePath2 });

            cancellationTokenSource.Cancel();

            await task;
        }
    }
}