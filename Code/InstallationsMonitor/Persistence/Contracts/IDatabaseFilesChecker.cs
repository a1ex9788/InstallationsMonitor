namespace Persistence.Contracts
{
    public interface IDatabaseFilesChecker
    {
        bool IsDatabaseFile(string path);
    }
}