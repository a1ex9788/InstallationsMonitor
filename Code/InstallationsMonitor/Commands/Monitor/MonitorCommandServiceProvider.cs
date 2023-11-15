using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class MonitorCommandServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        // Hook for tests.
        internal static Action<IServiceCollection>? ExtraRegistrationsAction;

        internal MonitorCommandServiceProvider(CancellationToken cancellationToken)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(typeof(CancellationToken), cancellationToken);

            services.AddPersistence();

            services.AddScoped<IMonitorCommand, MonitorCommand>();
            MonitorCommand.ConfigureSpecificServices(services);

            ExtraRegistrationsAction?.Invoke(services);

            this.serviceProvider = services.BuildServiceProvider();
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }
    }
}