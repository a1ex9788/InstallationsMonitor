﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests
{
    internal class TestUtilities
    {
        internal static string GetTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        internal static string GetTempFile()
        {
            return Path.GetTempFileName();
        }

        internal static async Task WaitForEventsRegistrationAsync()
        {
            await Task.Delay(100);
        }

        internal static async Task WaitForEventsProsecutionAsync(
            StringWriter stringWriter,
            IEnumerable<string>? expectedChangedFiles = null,
            IEnumerable<string>? expectedCreatedFiles = null,
            IEnumerable<string>? expectedNotCreatedFiles = null,
            IEnumerable<string>? expectedDeletedFiles = null,
            IEnumerable<(string OldPath, string NewPath)>? expectedRenamedFiles = null)
        {
            using CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            bool expectedResultsPrinted = false;
            do
            {
                try
                {
                    // Assert.
                    string output = stringWriter.ToString();

                    if (expectedChangedFiles is not null)
                    {
                        foreach (string expectedChangedFile in expectedChangedFiles)
                        {
                            output.Should().Contain($"Changed: {expectedChangedFile}");
                        }
                    }

                    if (expectedCreatedFiles is not null)
                    {
                        foreach (string expectedCreatedFile in expectedCreatedFiles)
                        {
                            output.Should().Contain($"Created: {expectedCreatedFile}");
                        }
                    }

                    if (expectedNotCreatedFiles is not null)
                    {
                        foreach (string expectedNotCreatedFile in expectedNotCreatedFiles)
                        {
                            output.Should().NotContain($"Created: {expectedNotCreatedFile}");
                        }
                    }

                    if (expectedDeletedFiles is not null)
                    {
                        foreach (string expectedDeletedFile in expectedDeletedFiles)
                        {
                            output.Should().Contain($"Deleted: {expectedDeletedFile}");
                        }
                    }

                    if (expectedRenamedFiles is not null)
                    {
                        foreach ((string OldPath, string NewPath) in expectedRenamedFiles)
                        {
                            output.Should().Contain($"Renamed: {OldPath} to {NewPath}");
                        }
                    }

                    expectedResultsPrinted = true;
                }
                catch
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        throw;
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                }
            } while (!expectedResultsPrinted);
        }
    }
}