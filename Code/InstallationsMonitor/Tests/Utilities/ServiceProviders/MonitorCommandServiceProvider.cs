using InstallationsMonitor.Commands.Monitor;
using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    internal class MonitorCommandServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        internal MonitorCommandServiceProvider() : this(new CancellationTokenSource().Token)
        {
        }

        internal MonitorCommandServiceProvider(CancellationToken cancellationToken)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(typeof(CancellationToken), cancellationToken);

            services.AddPersistence();
            ReplacePersistenceOptions(services);

            services.AddScoped<MonitorCommand>();

            MonitorCommand.ConfigureSpecificServices(services);

            this.serviceProvider = services.BuildServiceProvider();

            AppDbContext appDbContext = this.serviceProvider.GetRequiredService<AppDbContext>();
            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        private static void ReplacePersistenceOptions(IServiceCollection services)
        {
            string testDatabaseFullName = Path.Combine(
                TempPathUtilities.GetTempDirectory(), "TestDatabase.db");

            services.AddSingleton(new DatabaseOptions(testDatabaseFullName));
        }
    }
}