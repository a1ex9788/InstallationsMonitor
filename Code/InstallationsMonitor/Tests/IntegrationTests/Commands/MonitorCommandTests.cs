using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Logic.Contracts;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.ServiceProviders.Base;
using InstallationsMonitor.Tests.Utilities.ServiceProviders;
using InstallationsMonitor.TestsUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests.IntegrationTests.Commands
{
    [TestClass]
    public class MonitorCommandTests
    {
        [TestMethod]
        public async Task MonitorCommand_SomeFilesCreated_PrintsExpectedResults()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            string programName = "Program";
            string[] args = new string[] { "monitor", "-p", programName };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            DatabaseContext databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            CommandsServiceProvider.ExtraRegistrationsAction =
                sc =>
                {
                    sc.AddSingleton(typeof(CancellationToken), cancellationTokenSource.Token);
                    sc.AddSingleton(serviceProvider.GetRequiredService<DatabaseOptions>());
                };

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = Task.Run(() => Program.Main(args));

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath1 = Path.Combine(testPath, Guid.NewGuid().ToString());
            string filePath2 = TempPathsObtainer.GetTempFile();
            string[] filePaths = new string[] { filePath1, filePath2 };

            await File.Create(filePath1).DisposeAsync();
            await File.Create(filePath2).DisposeAsync();

            // Assert.
            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: filePaths);

            cancellationTokenSource.Cancel();
            await task;

            InstallationInfo installation = databaseContext.Installations.Single();
            installation.ProgramName.Should().Be(programName);

            databaseContext.FileCreations
                .Where(fc => fc.InstallationId == installation.Id)
                .Select(fc => fc.FilePath)
                .Should().BeEquivalentTo(filePaths);
        }

        [TestMethod]
        public void MonitorCommand_DirectoryNotSpecified_NullStringPassed()
        {
            // Arrange.
            string[] args = new string[] { "monitor" };

            string? directoryPassed = null;

            IMonitorCommand monitorCommand = Substitute.For<IMonitorCommand>();
            monitorCommand
                .ExecuteAsync(Arg.Any<string?>(), Arg.Any<string?>())
                .Returns(x =>
                {
                    directoryPassed = x.Args()[0] as string;

                    return Task.CompletedTask;
                });

            CommandsServiceProvider.ExtraRegistrationsAction = sc => sc.AddSingleton(monitorCommand);

            // Act.
            Program.Main(args);

            // Assert.
            directoryPassed.Should().Be(null);
        }

        [TestMethod]
        public void MonitorCommand_DirectorySpecified_DirectoryPassed()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            string[] args = new string[] { "monitor", "-d", testPath };

            string? directoryPassed = null;

            IMonitorCommand monitorCommand = Substitute.For<IMonitorCommand>();
            monitorCommand
                .ExecuteAsync(Arg.Any<string?>(), Arg.Any<string?>())
                .Returns(x =>
                {
                    directoryPassed = x.Args()[0] as string;

                    return Task.CompletedTask;
                });

            CommandsServiceProvider.ExtraRegistrationsAction = sc => sc.AddSingleton(monitorCommand);

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

            IMonitorCommand monitorCommand = Substitute.For<IMonitorCommand>();
            monitorCommand
                .ExecuteAsync(Arg.Any<string?>(), Arg.Any<string?>())
                .Returns(x =>
                {
                    programNamePassed = x.Args()[1] as string;

                    return Task.CompletedTask;
                });

            CommandsServiceProvider.ExtraRegistrationsAction = sc => sc.AddSingleton(monitorCommand);

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

            IMonitorCommand monitorCommand = Substitute.For<IMonitorCommand>();
            monitorCommand
                .ExecuteAsync(Arg.Any<string?>(), Arg.Any<string?>())
                .Returns(x =>
                {
                    programNamePassed = x.Args()[1] as string;

                    return Task.CompletedTask;
                });

            CommandsServiceProvider.ExtraRegistrationsAction = sc => sc.AddSingleton(monitorCommand);

            // Act.
            Program.Main(args);

            // Assert.
            programNamePassed.Should().Be(programName);
        }
    }
}