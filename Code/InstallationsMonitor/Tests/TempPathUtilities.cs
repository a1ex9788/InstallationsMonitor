using System;
using System.IO;

namespace InstallationsMonitor.Tests
{
    internal class TempPathUtilities
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
    }
}