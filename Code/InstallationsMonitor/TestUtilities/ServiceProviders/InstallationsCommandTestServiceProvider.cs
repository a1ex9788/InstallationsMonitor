using InstallationsMonitor.ServiceProviders.Installations;
using InstallationsMonitor.TestsUtilities.ServiceProviders.Base;
using System;
using System.Threading;

namespace InstallationsMonitor.TestsUtilities.ServiceProviders
{
    public class InstallationsCommandTestServiceProvider : CommandsTestServiceProvider
    {
        public InstallationsCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(GetServiceProvider, cancellationToken)
        {
        }

        private static IServiceProvider GetServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
        {
            return new InstallationsCommandServiceProvider(cancellationToken, databaseFullName);
        }
    }
}