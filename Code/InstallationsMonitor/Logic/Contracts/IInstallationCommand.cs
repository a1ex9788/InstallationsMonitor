namespace InstallationsMonitor.Logic.Contracts
{
    public interface IInstallationCommand
    {
        void Execute(int installationId);
    }
}