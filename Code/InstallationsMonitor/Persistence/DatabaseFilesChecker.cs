using Persistence.Contracts;
using System;

namespace InstallationsMonitor.Persistence
{
    public class DatabaseFilesChecker : IDatabaseFilesChecker
    {
        private readonly DatabaseOptions databaseOptions;

        public DatabaseFilesChecker(DatabaseOptions databaseOptions)
        {
            this.databaseOptions = databaseOptions;
        }

        public bool IsDatabaseFile(string path)
        {
            return path.StartsWith(this.databaseOptions.DatabaseFullName, StringComparison.Ordinal);
        }
    }
}