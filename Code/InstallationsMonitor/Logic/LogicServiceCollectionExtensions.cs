using InstallationsMonitor.Logic.Commands.Delete;
using InstallationsMonitor.Logic.Commands.Delete.Utilities;
using InstallationsMonitor.Logic.Commands.Installation.Utilities;
using InstallationsMonitor.Logic.Commands.Installations;
using InstallationsMonitor.Logic.Commands.Installations.Utilities;
using InstallationsMonitor.Logic.Commands.Monitor;
using InstallationsMonitor.Logic.Commands.Monitor.Utilities;
using InstallationsMonitor.Logic.Contracts;

using InstallationsMonitorClass =
    InstallationsMonitor.Logic.Commands.Monitor.Utilities.InstallationsMonitor;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LogicServiceCollectionExtensions
    {
        public static IServiceCollection AddDeleteCommand(this IServiceCollection services)
        {
            services.AddScoped<IDeleteCommand, DeleteCommand>();
            services.AddScoped<InstallationsDeleter>();

            return services;
        }

        public static IServiceCollection AddInstallationCommand(this IServiceCollection services)
        {
            services.AddScoped<IInstallationCommand, InstallationCommand>();
            services.AddScoped<InstallationPrinter>();

            return services;
        }

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

            return services;
        }
    }
}