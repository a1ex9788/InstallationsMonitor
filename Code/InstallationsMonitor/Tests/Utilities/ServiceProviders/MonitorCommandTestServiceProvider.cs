using InstallationsMonitor.Commands.Monitor;
using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    internal class MonitorCommandTestServiceProvider : IServiceProvider
    {
        private readonly MonitorCommandServiceProvider monitorCommandServiceProvider;

        internal MonitorCommandTestServiceProvider(CancellationToken cancellationToken)
        {
            string testDatabaseFullName = Path.Combine(
                TempPathsObtainer.GetTempDirectory(), "TestDatabase.db");

            MonitorCommandServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(new DatabaseOptions(testDatabaseFullName));

            this.monitorCommandServiceProvider = new MonitorCommandServiceProvider(cancellationToken);

            AppDbContext appDbContext = this.monitorCommandServiceProvider
                .GetRequiredService<AppDbContext>();

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
        }

        public object? GetService(Type serviceType)
        {
            return this.monitorCommandServiceProvider.GetService(serviceType);
        }
    }
}