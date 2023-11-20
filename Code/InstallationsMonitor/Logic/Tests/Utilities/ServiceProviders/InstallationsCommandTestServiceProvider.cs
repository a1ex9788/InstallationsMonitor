using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders.Base;
using InstallationsMonitor.ServiceProviders.Installations;
using System;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders
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