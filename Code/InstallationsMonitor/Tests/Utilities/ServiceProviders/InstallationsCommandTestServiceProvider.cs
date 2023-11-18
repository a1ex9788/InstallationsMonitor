using InstallationsMonitor.Commands.Installations;
using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    internal class InstallationsCommandTestServiceProvider : CommandsTestServiceProvider
    {
        internal InstallationsCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(ExtraRegistrations, GetServiceProvider, cancellationToken)
        {
        }

        private static void ExtraRegistrations(DatabaseOptions databaseOptions)
        {
            InstallationsCommandServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(databaseOptions);
        }

        private static IServiceProvider GetServiceProvider(CancellationToken cancellationToken)
        {
            return new InstallationsCommandServiceProvider(cancellationToken);
        }
    }
}