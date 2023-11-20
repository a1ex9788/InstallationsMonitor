using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders.Base;
using InstallationsMonitor.ServiceProviders.Monitor;
using System;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders
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