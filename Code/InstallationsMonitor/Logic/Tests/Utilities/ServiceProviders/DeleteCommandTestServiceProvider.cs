using InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders.Base;
using InstallationsMonitor.ServiceProviders.Delete;
using System;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders
{
    public class DeleteCommandTestServiceProvider : CommandsTestServiceProvider
    {
        public DeleteCommandTestServiceProvider(CancellationToken cancellationToken)
            : base(GetServiceProvider, cancellationToken)
        {
        }

        private static IServiceProvider GetServiceProvider(
            CancellationToken cancellationToken, string databaseFullName)
        {
            return new DeleteCommandServiceProvider(cancellationToken, databaseFullName);
        }
    }
}