using InstallationsMonitor.Commands.Remove;
using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    internal class RemoveCommandTestServiceProvider : CommandsTestServiceProvider
    {
        internal RemoveCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(ExtraRegistrations, GetServiceProvider, cancellationToken)
        {
        }

        private static void ExtraRegistrations(DatabaseOptions databaseOptions)
        {
            RemoveCommandServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(databaseOptions);
        }

        private static IServiceProvider GetServiceProvider(CancellationToken cancellationToken)
        {
            return new RemoveCommandServiceProvider(cancellationToken);
        }
    }
}