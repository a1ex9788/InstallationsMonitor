using FluentAssertions;
using InstallationsMonitor.Commands.Monitor;
using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests.UnitTests.Commands.Monitor
{
    [TestClass]
    public class DirectoriesMonitorTests
    {
        [TestMethod]
        public async Task MonitorAsync_FileChanged_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            int installationId = 1;
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Get get = new Get();
            using DatabaseConnection databaseConnection = get.DatabaseConnection;
            DirectoriesMonitor directoriesMonitor = get.DirectoriesMonitor;

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(
                testPath, installationId, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync(stringWriter);

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

            FileChange fileChange = (FileChange)databaseConnection.GetFileOperations().ElementAt(1);
            fileChange.FilePath.Should().Be(filePath);
            fileChange.InstallationId.Should().Be(installationId);
        }

        [TestMethod]
        public async Task MonitorAsync_FileCreated_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            int installationId = 1;
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Get get = new Get();
            using DatabaseConnection databaseConnection = get.DatabaseConnection;
            DirectoriesMonitor directoriesMonitor = get.DirectoriesMonitor;

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(
                testPath, installationId, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync(stringWriter);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();

            // Assert.
            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedCreatedFiles: new string[] { filePath });

            cancellationTokenSource.Cancel();
            await task;

            FileCreation fileCreation = (FileCreation)databaseConnection.GetFileOperations().Single();
            fileCreation.FilePath.Should().Be(filePath);
            fileCreation.InstallationId.Should().Be(installationId);
        }

        [TestMethod]
        public async Task MonitorAsync_FileDeleted_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            int installationId = 1;
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Get get = new Get();
            using DatabaseConnection databaseConnection = get.DatabaseConnection;
            DirectoriesMonitor directoriesMonitor = get.DirectoriesMonitor;

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(
                testPath, installationId, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync(stringWriter);

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

            FileDeletion fileDeletion = (FileDeletion)databaseConnection.GetFileOperations().ElementAt(1);
            fileDeletion.FilePath.Should().Be(filePath);
            fileDeletion.InstallationId.Should().Be(installationId);
        }

        [TestMethod]
        public async Task MonitorAsync_FileRenamed_CreatesFileOperation()
        {
            // Arrange.
            string testPath = TempPathUtilities.GetTempDirectory();
            int installationId = 1;
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Get get = new Get();
            using DatabaseConnection databaseConnection = get.DatabaseConnection;
            DirectoriesMonitor directoriesMonitor = get.DirectoriesMonitor;

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(
                testPath, installationId, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync(stringWriter);

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

            FileRenaming fileRenaming = (FileRenaming)databaseConnection.GetFileOperations().ElementAt(1);
            fileRenaming.FilePath.Should().Be(newFilePath);
            fileRenaming.InstallationId.Should().Be(installationId);
            fileRenaming.OldPath.Should().Be(filePath);
        }

        [TestMethod]
        public async Task MonitorAsync_DatabaseOperations_DatabaseFilesNotAnalysed()
        {
            // Arrange.
            string testPath = Directory.GetParent(DatabaseUtilities.TestDatabaseFullName)!.FullName;
            int installationId = 1;
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Get get = new Get();
            using DatabaseConnection databaseConnection = get.DatabaseConnection;
            DirectoriesMonitor directoriesMonitor = get.DirectoriesMonitor;

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Task task = directoriesMonitor.MonitorAsync(
                testPath, installationId, cancellationTokenSource.Token);

            await EventsUtilities.WaitForEventsRegistrationAsync(stringWriter);

            string filePath = Path.Combine(testPath, Guid.NewGuid().ToString());
            string newFilePath = Path.Combine(testPath, Guid.NewGuid().ToString());

            await File.Create(filePath).DisposeAsync();
            File.WriteAllText(filePath, string.Empty);
            File.Move(filePath, newFilePath);
            File.Delete(newFilePath);

            // Assert.
            await EventsUtilities.WaitForEventsProsecutionAsync(
                stringWriter,
                expectedChangedFiles: new string[] { filePath },
                expectedCreatedFiles: new string[] { filePath },
                expectedDeletedFiles: new string[] { newFilePath },
                expectedRenamedFiles: new (string OldPath, string NewPath)[] { (filePath, newFilePath) });

            cancellationTokenSource.Cancel();
            await task;

            stringWriter.ToString().Should().NotContain(DatabaseUtilities.TestDatabaseFullName);

            IEnumerable<FileOperation> fileOperations = databaseConnection.GetFileOperations();
            fileOperations.Should().HaveCount(4);
            fileOperations.FirstOrDefault(
                fo => fo.FilePath == DatabaseUtilities.TestDatabaseFullName).Should().BeNull();
        }
    }
}