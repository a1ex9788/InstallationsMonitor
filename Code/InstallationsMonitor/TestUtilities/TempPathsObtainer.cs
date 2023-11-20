using System;
using System.IO;

namespace InstallationsMonitor.TestsUtilities
{
    public class TempPathsObtainer
    {
        public static string GetTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        public static string GetTempFile()
        {
            return Path.GetTempFileName();
        }
    }
}