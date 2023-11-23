namespace InstallationsMonitor.Logic.Contracts
{
    public interface IDeleteCommand
    {
        void Execute(int installationId);
    }
}