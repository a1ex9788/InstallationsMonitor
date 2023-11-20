using InstallationsMonitor.ServiceProviders.Monitor;
using InstallationsMonitor.Tests.Utilities.ServiceProviders.Base;
using System;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    public class MonitorCommandTestServiceProvider : CommandsTestServiceProvider
    {
        public MonitorCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(GetServiceProvider, cancellationToken)
        {
        }

        private static IServiceProvider GetServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
        {
            return new MonitorCommandServiceProvider(cancellationToken, databaseFullName);
        }
    }
}