using InstallationsMonitor.Commands.Installations;
using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    internal class InstallationsCommandTestServiceProvider : IServiceProvider
    {
        private readonly InstallationsCommandServiceProvider installationsCommandServiceProvider;

        internal InstallationsCommandTestServiceProvider(CancellationToken cancellationToken)
        {
            string testDatabaseFullName = Path.Combine(
                TempPathsObtainer.GetTempDirectory(), "TestDatabase.db");

            InstallationsCommandServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(new DatabaseOptions(testDatabaseFullName));

            this.installationsCommandServiceProvider = new InstallationsCommandServiceProvider(
                cancellationToken);

            AppDbContext appDbContext = this.installationsCommandServiceProvider
                .GetRequiredService<AppDbContext>();

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
        }

        public object? GetService(Type serviceType)
        {
            return this.installationsCommandServiceProvider.GetService(serviceType);
        }
    }
}