using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Commands.Installations
{
    internal class InstallationsCommandServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;

        // Hook for tests.
        internal static Action<IServiceCollection>? ExtraRegistrationsAction;

        internal InstallationsCommandServiceProvider(CancellationToken cancellationToken)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(typeof(CancellationToken), cancellationToken);

            services.AddPersistence();

            services.AddScoped<IInstallationsCommand, InstallationsCommand>();
            InstallationsCommand.ConfigureSpecificServices(services);

            ExtraRegistrationsAction?.Invoke(services);

            this.serviceProvider = services.BuildServiceProvider();
        }

        public object? GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }
    }
}