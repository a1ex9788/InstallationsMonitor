using System.Threading.Tasks;

namespace InstallationsMonitor.Logic.Contracts
{
    public interface IMonitorCommand
    {
        Task ExecuteAsync(string? directory, string? programName);
    }
}