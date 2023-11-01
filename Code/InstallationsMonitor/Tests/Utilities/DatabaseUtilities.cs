using InstallationsMonitor.Persistence;
using System.IO;

namespace InstallationsMonitor.Tests.Utilities
{
    internal static class DatabaseUtilities
    {
        internal static AppDbContext GetTestAppDbContext()
        {
            string testDatabaseFullName = Path.Combine(
                TempPathUtilities.GetTempDirectory(), "TestDatabase.db");

            DatabaseOptions databaseOptions = new DatabaseOptions(testDatabaseFullName);
            AppDbContext appDbContext = new AppDbContext(databaseOptions);

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();

            return appDbContext;
        }
    }
}