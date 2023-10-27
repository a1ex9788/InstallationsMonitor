using InstallationsMonitor.Commands;
using InstallationsMonitor.Commands.Monitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public async Task MonitorCommandForAllDrives_SomeFilesCreated_PrintsExpectedResults()
        {
            // Arrange.
            string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testPath);

            string[] args = new string[] { "monitor" };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            CommandsCreator.CreateMonitorcommandImplementation =
                (directory, _) => new MonitorCommand(directory, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => Program.Main(args));

            await TestUtilities.WaitForEventsRegistrationAsync();

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
        public async Task MonitorCommandForConcretePath_SomeFilesCreated_PrintsExpectedResults()
        {
            // Arrange.
            string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testPath);

            string[] args = new string[] { "monitor", "-d", testPath };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            CommandsCreator.CreateMonitorcommandImplementation =
                (directory, _) => new MonitorCommand(directory, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => Program.Main(args));

            await TestUtilities.WaitForEventsRegistrationAsync();

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