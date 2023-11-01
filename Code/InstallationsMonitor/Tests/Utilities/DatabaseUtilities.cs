using InstallationsMonitor.Persistence;
using System.IO;

namespace InstallationsMonitor.Tests.Utilities
{
    internal static class DatabaseUtilities
    {
        internal static DatabaseConnection GetTestDatabaseConnection()
        {
            string testDatabaseFullName = Path.Combine(
                TempPathUtilities.GetTempDirectory(), "TestDatabase.db");

            DatabaseOptions databaseOptions = new DatabaseOptions(testDatabaseFullName);
            AppDbContext appDbContext = new AppDbContext(databaseOptions);
            DatabaseConnection databaseConnection = new DatabaseConnection(appDbContext);

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();

            return databaseConnection;
        }
    }
}