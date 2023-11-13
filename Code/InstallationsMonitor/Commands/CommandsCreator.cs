using InstallationsMonitor.Commands.Monitor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace InstallationsMonitor.Commands
{
    internal static class CommandsCreator
    {
        // Hooks for tests.
        internal static Action<IServiceCollection>? ExtraRegistrationsAction;

        internal static Func<IServiceProvider, string?, string?, ICommand> CreateMonitorCommandFunction =
            (sp, d, pn) => new MonitorCommand(sp.GetRequiredService<Monitor.InstallationsMonitor>(), d, pn);

        internal static ICommand CreateMonitorCommand(
            string? directory, string? programName, CancellationToken cancellationToken)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(typeof(CancellationToken), cancellationToken);

            services.AddPersistence();

            services.AddSingleton(
                typeof(ICommand),
                sp => CreateMonitorCommandFunction.Invoke(sp, directory, programName));

            MonitorCommand.ConfigureSpecificServices(services);

            ExtraRegistrationsAction?.Invoke(services);

            return services.BuildServiceProvider().GetRequiredService<ICommand>();
        }
    }
}