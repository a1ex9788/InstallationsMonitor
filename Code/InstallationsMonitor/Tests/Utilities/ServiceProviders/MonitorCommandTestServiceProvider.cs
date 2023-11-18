using InstallationsMonitor.Commands.Monitor;
using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    internal class MonitorCommandTestServiceProvider : CommandsTestServiceProvider
    {
        internal MonitorCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(ExtraRegistrations, GetServiceProvider, cancellationToken)
        {
        }

        private static void ExtraRegistrations(DatabaseOptions databaseOptions)
        {
            MonitorCommandServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(databaseOptions);
        }

        private static IServiceProvider GetServiceProvider(CancellationToken cancellationToken)
        {
            return new MonitorCommandServiceProvider(cancellationToken);
        }
    }
}