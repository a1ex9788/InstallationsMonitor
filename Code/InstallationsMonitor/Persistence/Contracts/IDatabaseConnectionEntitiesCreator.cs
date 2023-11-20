using InstallationsMonitor.Domain;

namespace InstallationsMonitor.Persistence.Contracts
{
    public interface IDatabaseConnectionEntitiesCreator
    {
        int CreateInstallation(Installation installation);

        void CreateFileChange(FileChange fileChange);

        void CreateFileCreation(FileCreation fileCreation);

        void CreateFileDeletion(FileDeletion fileDeletion);

        void CreateFileRenaming(FileRenaming fileRenaming);
    }
}