using InstallationsMonitor.Commands.Monitor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Commands
{
    internal static class CommandsCreator
    {
        // Hook for tests.
        internal static Action<IServiceCollection>? ExtraRegistrationsAction;

        internal static IMonitorCommand CreateMonitorCommand(CancellationToken cancellationToken)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(typeof(CancellationToken), cancellationToken);

            services.AddPersistence();

            services.AddScoped<IMonitorCommand, MonitorCommand>();
            MonitorCommand.ConfigureSpecificServices(services);

            ExtraRegistrationsAction?.Invoke(services);

            return services.BuildServiceProvider().GetRequiredService<IMonitorCommand>();
        }
    }
}