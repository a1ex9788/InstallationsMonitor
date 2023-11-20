namespace InstallationsMonitor.Logic.Commands.Remove
{
    public interface IRemoveCommand
    {
        void Execute(int installationId);
    }
}