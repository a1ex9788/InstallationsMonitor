using FluentAssertions;
using InstallationsMonitor.Commands;
using InstallationsMonitor.Commands.Monitor;
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
            string[] args = new string[] { "monitor" };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            CommandsCreator.CreateMonitorCommandImplementation =
                (directory, _) => new MonitorCommand(directory, cancellationTokenSource.Token);

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => Program.Main(args));

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
        public void MonitorCommand_DirectoryNotSpecified_NullStringPassed()
        {
            // Arrange.
            string[] args = new string[] { "monitor" };

            string? directory = null;

            CommandsCreator.CreateMonitorCommandImplementation = (d, _) =>
            {
                directory = d;

                return new Mock<ICommand>().Object;
            };

            // Act.
            Program.Main(args);

            // Assert.
            directory.Should().Be(null);
        }

        [TestMethod]
        public void MonitorCommand_DirectorySpecified_DirectoryPassed()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            string[] args = new string[] { "monitor", "-d", testPath };

            string? directory = null;

            CommandsCreator.CreateMonitorCommandImplementation = (d, _) =>
            {
                directory = d;

                return new Mock<ICommand>().Object;
            };

            // Act.
            Program.Main(args);

            // Assert.
            directory.Should().Be(testPath);
        }
    }
}