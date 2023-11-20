using InstallationsMonitor.Persistence;
using InstallationsMonitor.ServiceProviders.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.TestsUtilities.ServiceProviders.Base
{
    public class CommandsTestServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        public CommandsTestServiceProvider(
            Func<CancellationToken, string, IServiceProvider> getServiceCollectionFunc,
            CancellationToken cancellationToken)
        {
            string testDatabaseFullName = Path.Combine(
                TempPathsObtainer.GetTempDirectory(), "TestDatabase.db");

            DatabaseOptions databaseOptions = new DatabaseOptions(testDatabaseFullName);

            CommandsServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(databaseOptions);

            this.serviceProvider = getServiceCollectionFunc.Invoke(
                cancellationToken, testDatabaseFullName);

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