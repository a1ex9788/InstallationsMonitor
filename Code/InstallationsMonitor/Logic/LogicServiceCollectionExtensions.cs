using InstallationsMonitor.Logic.Commands.Installations;
using InstallationsMonitor.Logic.Commands.Installations.Utilities;
using InstallationsMonitor.Logic.Commands.Monitor;
using InstallationsMonitor.Logic.Commands.Monitor.Utilities;
using InstallationsMonitor.Logic.Commands.Remove;
using InstallationsMonitor.Logic.Commands.Remove.Utilities;
using InstallationsMonitor.Logic.Contracts;
using InstallationsMonitor.Persistence;

using InstallationsMonitorClass =
    InstallationsMonitor.Logic.Commands.Monitor.Utilities.InstallationsMonitor;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LogicServiceCollectionExtensions
    {
        public static IServiceCollection AddInstallationsCommand(this IServiceCollection services)
        {
            services.AddScoped<IInstallationsCommand, InstallationsCommand>();
            services.AddScoped<InstallationsPrinter>();
            services.AddScoped<InstallationsObtainer>();

            return services;
        }

        public static IServiceCollection AddMonitorCommand(this IServiceCollection services)
        {
            services.AddScoped<IMonitorCommand, MonitorCommand>();
            services.AddScoped<InstallationsMonitorClass>();
            services.AddScoped<DirectoriesMonitor>();
            services.AddScoped<DatabaseFilesChecker>();

            return services;
        }

        public static IServiceCollection AddRemoveCommand(this IServiceCollection services)
        {
            services.AddScoped<IRemoveCommand, RemoveCommand>();
            services.AddScoped<InstallationsRemover>();

            return services;
        }
    }
}