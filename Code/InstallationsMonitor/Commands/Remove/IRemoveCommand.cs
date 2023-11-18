namespace InstallationsMonitor.Commands.Remove
{
    internal interface IRemoveCommand
    {
        void Execute(int installationId);
    }
}