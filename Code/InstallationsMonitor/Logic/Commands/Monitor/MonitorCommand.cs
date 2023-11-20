using System.Threading.Tasks;

namespace InstallationsMonitor.Logic.Commands.Monitor
{
    public class MonitorCommand : IMonitorCommand
    {
        private readonly Utilities.InstallationsMonitor installationsMonitor;

        public MonitorCommand(Utilities.InstallationsMonitor installationsMonitor)
        {
            this.installationsMonitor = installationsMonitor;
        }

        public async Task ExecuteAsync(string? directory, string? programName)
        {
            await this.installationsMonitor.MonitorAsync(directory, programName);
        }
    }
}