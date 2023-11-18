using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.ServiceProviders
{
    internal abstract class CommandsServiceProvider<T, U> : IServiceProvider
        where T : class
        where U : class, T
    {
        private readonly IServiceProvider serviceProvider;

        // Hook for tests.
        internal static Action<IServiceCollection>? ExtraRegistrationsAction;

        internal CommandsServiceProvider(
            Action<IServiceCollection> configureSpecificServicesAction,
            CancellationToken cancellationToken)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(typeof(CancellationToken), cancellationToken);

            services.AddPersistence();

            services.AddScoped<T, U>();
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