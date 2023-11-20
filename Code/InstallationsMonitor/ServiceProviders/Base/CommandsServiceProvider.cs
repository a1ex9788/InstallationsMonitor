using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.ServiceProviders.Base
{
    public abstract class CommandsServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        // Hook for tests.
        public static Action<IServiceCollection>? ExtraRegistrationsAction;

        public CommandsServiceProvider(
            Action<IServiceCollection> configureSpecificServicesAction,
            CancellationToken cancellationToken,
            string databaseFullName)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(typeof(CancellationToken), cancellationToken);

            services.AddPersistence(databaseFullName);

            configureSpecificServicesAction.Invoke(services);

            ExtraRegistrationsAction?.Invoke(services);

            this.serviceProvider = services.BuildServiceProvider();
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }
    }
}