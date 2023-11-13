using InstallationsMonitor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class MonitorCommand : ICommand
    {
        private readonly InstallationsMonitor installationsMonitor;

        private readonly string? directory;
        private readonly string? programName;

        internal MonitorCommand(
            InstallationsMonitor installationsMonitor, string? directory, string? programName)
        {
            this.installationsMonitor = installationsMonitor;

            this.directory = directory;
            this.programName = programName;
        }

        internal static void ConfigureSpecificServices(IServiceCollection services)
        {
            services.AddScoped<InstallationsMonitor>();
            services.AddScoped<DirectoriesMonitor>();
            services.AddScoped<DatabaseFilesChecker>();
        }

        public async Task ExecuteAsync()
        {
            await this.installationsMonitor.MonitorAsync(this.directory, this.programName);
        }
    }
}