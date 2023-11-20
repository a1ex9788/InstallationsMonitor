namespace InstallationsMonitor.Logic.Contracts
{
    public interface IRemoveCommand
    {
        void Execute(int installationId);
    }
}