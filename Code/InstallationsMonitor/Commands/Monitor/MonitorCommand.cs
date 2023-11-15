using InstallationsMonitor.Commands.Monitor.Utilities;
using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class MonitorCommand : IMonitorCommand
    {
        private readonly Utilities.InstallationsMonitor installationsMonitor;

        public MonitorCommand(Utilities.InstallationsMonitor installationsMonitor)
        {
            this.installationsMonitor = installationsMonitor;
        }

        internal static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddScoped<Utilities.InstallationsMonitor>();
            services.AddScoped<DirectoriesMonitor>();
            services.AddScoped<DatabaseFilesChecker>();
        }

        public async Task ExecuteAsync(string? directory, string? programName)
        {
            await this.installationsMonitor.MonitorAsync(directory, programName);
        }
    }
}