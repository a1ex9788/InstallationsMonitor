using InstallationsMonitor.Domain;
using System.Collections.Generic;

namespace InstallationsMonitor.Persistence.Contracts
{
    public interface IDatabaseConnection
    {
        int CreateInstallation(Installation installation);

        void CreateFileChange(FileChange fileChange);

        void CreateFileCreation(FileCreation fileCreation);

        void CreateFileDeletion(FileDeletion fileDeletion);

        void CreateFileRenaming(FileRenaming fileRenaming);

        IEnumerable<Installation> GetInstallations();

        Installation? GetInstallation(int installationId);

        IEnumerable<FileChange> GetFileChanges();

        IEnumerable<FileCreation> GetFileCreations();

        IEnumerable<FileDeletion> GetFileDeletions();

        IEnumerable<FileRenaming> GetFileRenamings();

        void RemoveInstallation(int installationId);

        void RemoveFileOperations(int installationId);
    }
}