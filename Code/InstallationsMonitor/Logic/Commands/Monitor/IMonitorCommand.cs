using System.Threading.Tasks;

namespace InstallationsMonitor.Logic.Commands.Monitor
{
    public interface IMonitorCommand
    {
        Task ExecuteAsync(string? directory, string? programName);
    }
}