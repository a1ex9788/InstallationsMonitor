using System;
using System.IO;

namespace InstallationsMonitor
{
    internal static class Settings
    {
        private const string DatabaseName = "InstallationsMonitor.db";

        internal static string GetDatabaseFullName()
        {
            if (Environment.ProcessPath == null)
            {
                return DatabaseName;
            }

            DirectoryInfo? directory = Directory.GetParent(Environment.ProcessPath);

            if (directory == null)
            {
                return DatabaseName;
            }

            return Path.Combine(directory.FullName, DatabaseName);
        }
    }
}