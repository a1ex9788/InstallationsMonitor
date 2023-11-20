namespace InstallationsMonitor.Persistence.Contracts
{
    public interface IDatabaseConnectionEntitiesRemover
    {
        void RemoveInstallation(int installationId);
    }
}