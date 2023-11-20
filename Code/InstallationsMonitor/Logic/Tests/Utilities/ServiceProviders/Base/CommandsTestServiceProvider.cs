using InstallationsMonitor.Persistence.Contracts;
using InstallationsMonitor.ServiceProviders.Base;
using Logic.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Logic.Tests.Utilities.ServiceProviders.Base
{
    public class CommandsTestServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        public CommandsTestServiceProvider(
            Func<CancellationToken, string, IServiceProvider> getServiceCollectionFunc,
            CancellationToken cancellationToken)
        {
            CommandsServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton<IDatabaseConnection, FakeDatabaseConnection>();

            this.serviceProvider = getServiceCollectionFunc.Invoke(cancellationToken, "FakeDatabase");
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }
    }
}