using System.Threading.Tasks;

namespace InstallationsMonitor.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}