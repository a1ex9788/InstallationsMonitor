using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders.Base;
using InstallationsMonitor.ServiceProviders.Installation;
using System;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders
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