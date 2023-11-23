using InstallationsMonitor.ServiceProviders.Installation;
using InstallationsMonitor.Tests.Utilities.ServiceProviders.Base;
using System;
using System.Threading;

namespace InstallationsMonitor.Tests.Utilities.ServiceProviders
{
    public class InstallationCommandTestServiceProvider : CommandsTestServiceProvider
    {
        public InstallationCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(GetServiceProvider, cancellationToken)
        {
        }

        private static IServiceProvider GetServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
        {
            return new InstallationCommandServiceProvider(cancellationToken, databaseFullName);
        }
    }
}