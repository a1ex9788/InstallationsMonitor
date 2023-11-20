using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders.Base;
using InstallationsMonitor.ServiceProviders.Remove;
using System;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders
{
    public class RemoveCommandTestServiceProvider : CommandsTestServiceProvider
    {
        public RemoveCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(GetServiceProvider, cancellationToken)
        {
        }

        private static IServiceProvider GetServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
        {
            return new RemoveCommandServiceProvider(cancellationToken, databaseFullName);
        }
    }
}