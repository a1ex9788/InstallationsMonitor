using InstallationsMonitor.Persistence;
using InstallationsMonitor.ServiceProviders.Base;
using InstallationsMonitor.TestsUtilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders.Base
{
    public class CommandsTestServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        public CommandsTestServiceProvider(
            Func<CancellationToken, string, IServiceProvider> getServiceCollectionFunc,
            CancellationToken cancellationToken)
        {
            string testDatabaseFullName = Path.Combine(
                TempPathsObtainer.GetTempDirectory(), $"TestDatabase.{Guid.NewGuid()}.db");

            DatabaseOptions databaseOptions = new DatabaseOptions(testDatabaseFullName);

            CommandsServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(databaseOptions);

            this.serviceProvider = getServiceCollectionFunc.Invoke(
                cancellationToken, testDatabaseFullName);

            DatabaseContext databaseContext = this.serviceProvider.GetRequiredService<DatabaseContext>();

            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }
    }
}