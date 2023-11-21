using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Domain.Base;
using InstallationsMonitor.Logic.Commands.Monitor.Utilities;
using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Persistence.Contracts;
using InstallationsMonitor.TestsUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Logic.Tests.UnitTests.Commands.Monitor
{
    [TestClass]
    public class DirectoriesMonitorTests
    {
        [TestMethod]
        public async Task MonitorAsync_FileChanged_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            int installationId = 1;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            DirectoriesMonitor directoriesMonitor = serviceProvider
                .GetRequiredService<DirectoriesMonitor>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(testPath, installationId);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.WriteAllText(filePath, string.Empty);

            // Assert.
            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedChangedFiles: new string[] { filePath },
                expectedCreatedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();
            await task;

            IEnumerable<FileChange> fileChanges = databaseConnection.GetFileChanges();
            fileChanges.Should().HaveCount(1);
            FileChange fileChange = fileChanges.Single();
            fileChange.FilePath.Should().Be(filePath);
            fileChange.InstallationId.Should().Be(installationId);
        }

        [TestMethod]
        public async Task MonitorAsync_FileCreated_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            int installationId = 1;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            DirectoriesMonitor directoriesMonitor = serviceProvider
                .GetRequiredService<DirectoriesMonitor>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(testPath, installationId);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();

            // Assert.
            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();
            await task;

            IEnumerable<FileCreation> fileCreations = databaseConnection.GetFileCreations();
            fileCreations.Should().HaveCount(1);
            FileCreation fileCreation = fileCreations.Single();
            fileCreation.FilePath.Should().Be(filePath);
            fileCreation.InstallationId.Should().Be(installationId);
        }

        [TestMethod]
        public async Task MonitorAsync_FileDeleted_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            int installationId = 1;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            DirectoriesMonitor directoriesMonitor = serviceProvider
                .GetRequiredService<DirectoriesMonitor>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(testPath, installationId);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.Delete(filePath);

            // Assert.

            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath },
                expectedDeletedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();
            await task;

            IEnumerable<FileDeletion> fileDeletions = databaseConnection.GetFileDeletions();
            fileDeletions.Should().HaveCount(1);
            FileDeletion fileDeletion = fileDeletions.Single();
            fileDeletion.FilePath.Should().Be(filePath);
            fileDeletion.InstallationId.Should().Be(installationId);
        }

        [TestMethod]
        public async Task MonitorAsync_FileRenamed_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathsObtainer.GetTempDirectory();
            int installationId = 1;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            DirectoriesMonitor directoriesMonitor = serviceProvider
                .GetRequiredService<DirectoriesMonitor>();

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(testPath, installationId);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());
            string newFilePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.Move(filePath, newFilePath);

            // Assert.
            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath },
                expectedRenamedFiles: new (string OldPath, string NewPath)[] { (filePath, newFilePath) });

            cancellationTokenSource.Cancel();
            await task;

            IEnumerable<FileRenaming> fileRenamings = databaseConnection.GetFileRenamings();
            fileRenamings.Should().HaveCount(1);
            FileRenaming fileRenaming = fileRenamings.Single();
            fileRenaming.FilePath.Should().Be(newFilePath);
            fileRenaming.InstallationId.Should().Be(installationId);
            fileRenaming.OldPath.Should().Be(filePath);
        }

        [TestMethod]
        public async Task MonitorAsync_DatabaseOperations_DatabaseFilesNotAnalysed()
        {
            // Arrange.
            int installationId = 1;

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new MonitorCommandTestServiceProvider(
                cancellationTokenSource.Token);
            IDatabaseConnection databaseConnection = serviceProvider
                .GetRequiredService<IDatabaseConnection>();
            DirectoriesMonitor directoriesMonitor = serviceProvider
                .GetRequiredService<DirectoriesMonitor>();

            DatabaseOptions databaseOptions = serviceProvider.GetRequiredService<DatabaseOptions>();
            string testPath = Directory.GetParent(databaseOptions.DatabaseFullName)!.FullName;

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(testPath, installationId);

            await EventsAwaiter.WaitForEventsRegistrationAsync(stringWriter);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());
            string newFilePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.WriteAllText(filePath, string.Empty);
            File.Move(filePath, newFilePath);
            File.Delete(newFilePath);

            // Assert.
            await EventsAwaiter.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedChangedFiles: new string[] { filePath },
                expectedCreatedFiles: new string[] { filePath },
                expectedDeletedFiles: new string[] { newFilePath },
                expectedRenamedFiles: new (string OldPath, string NewPath)[] { (filePath, newFilePath) });

            cancellationTokenSource.Cancel();
            await task;

            stringWriter.ToString().Should().NotContain(databaseOptions.DatabaseFullName);

            IEnumerable<FileOperation> fileChanges = databaseConnection.GetFileChanges();
            IEnumerable<FileOperation> fileCreations = databaseConnection.GetFileCreations();
            IEnumerable<FileOperation> fileDeletions = databaseConnection.GetFileDeletions();
            IEnumerable<FileOperation> fileRenamings = databaseConnection.GetFileRenamings();
            IList<FileOperation> fileOperations = fileChanges
                .Concat(fileCreations).Concat(fileDeletions).Concat(fileRenamings).ToList();
            fileOperations.Should().HaveCount(4);
            fileOperations
                .Where(
                    fo => fo.FilePath.Contains(databaseOptions.DatabaseFullName, StringComparison.Ordinal))
                .Should().BeEmpty();
        }
    }
}