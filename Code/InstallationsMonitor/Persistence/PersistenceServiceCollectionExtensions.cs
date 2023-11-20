using InstallationsMonitor.Persistence;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, string databaseFullName)
        {
            services.AddDbContext<AppDbContext>();
            services.AddSingleton<DatabaseConnection>();
            services.AddSingleton(new DatabaseOptions(databaseFullName));

            return services;
        }
    }
}