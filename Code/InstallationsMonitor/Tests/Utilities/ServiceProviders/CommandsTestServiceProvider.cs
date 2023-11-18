using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    internal class CommandsTestServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        internal CommandsTestServiceProvider(
            Action<DatabaseOptions> extraRegistrationsAction,
            Func<CancellationToken, IServiceProvider> getServiceCollectionFunc,
            CancellationToken cancellationToken)
        {
            string TestDatabaseFullName = Path.Combine(
                TempPathsObtainer.GetTempDirectory(), "TestDatabase.db");

            DatabaseOptions databaseOptions = new DatabaseOptions(TestDatabaseFullName);

            extraRegistrationsAction.Invoke(databaseOptions);

            this.serviceProvider = getServiceCollectionFunc.Invoke(cancellationToken);

            AppDbContext appDbContext = this.serviceProvider.GetRequiredService<AppDbContext>();

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }
    }
}