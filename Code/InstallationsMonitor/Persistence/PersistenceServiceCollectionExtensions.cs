using InstallationsMonitor;
using InstallationsMonitor.Persistence;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class PersistenceServiceCollectionExtensions
    {
        internal static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddSingleton<DatabaseConnection>();
            services.AddSingleton(new DatabaseOptions(Settings.GetDatabaseFullName()));

            return services;
        }
    }
}