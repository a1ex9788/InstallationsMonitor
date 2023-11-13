using System;

namespace InstallationsMonitor.Persistence
{
    internal class DatabaseFilesChecker
    {
        private readonly DatabaseOptions databaseOptions;

        public DatabaseFilesChecker(DatabaseOptions databaseOptions)
        {
            this.databaseOptions = databaseOptions;
        }

        internal bool IsDatabaseFile(string path)
        {
            return path.StartsWith(this.databaseOptions.DatabaseFullName, StringComparison.Ordinal);
        }
    }
}