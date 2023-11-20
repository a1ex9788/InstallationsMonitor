using InstallationsMonitor.Persistence;
using InstallationsMonitor.Persistence.Contracts;
using Persistence.Contracts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, string databaseFullName)
        {
            services.AddDbContext<AppDbContext>();

            services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

            services.AddSingleton(new DatabaseOptions(databaseFullName));

            return services;
        }

        public static IServiceCollection AddDatabaseFilesChecker(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseFilesChecker, DatabaseFilesChecker>();

            return services;
        }
    }
}