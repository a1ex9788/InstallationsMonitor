using InstallationsMonitor;
using InstallationsMonitor.Persistence;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class PersistenceServiceCollectionExtensions
    {
        internal static Func<IServiceCollection, IServiceCollection>
            AddPersistenceImplementation = services =>
            {
                services.AddDbContext<AppDbContext>();
                services.AddSingleton<DatabaseConnection>();
                services.AddSingleton(new DatabaseOptions(Settings.GetDatabaseFullName()));

                return services;
            };

        internal static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return AddPersistenceImplementation(services);
        }
    }
}