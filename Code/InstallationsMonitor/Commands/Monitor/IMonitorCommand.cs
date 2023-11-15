using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal interface IMonitorCommand
    {
        Task ExecuteAsync(string? directory, string? programName);
    }
}