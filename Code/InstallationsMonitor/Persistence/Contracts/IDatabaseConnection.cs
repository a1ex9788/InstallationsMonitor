using InstallationsMonitor.Domain;
using System.Collections.Generic;

namespace InstallationsMonitor.Persistence.Contracts
{
    public interface IDatabaseConnection
    {
        void CreateFileChange(FileChange fileChange);

        void CreateFileCreation(FileCreation fileCreation);

        void CreateFileDeletion(FileDeletion fileDeletion);

        void CreateFileRenaming(FileRenaming fileRenaming);

        int CreateInstallation(InstallationInfo installation);

        void DeleteFileOperations(int installationId);

        void DeleteInstallation(int installationId);

        IEnumerable<FileChange> GetFileChanges();

        IEnumerable<FileCreation> GetFileCreations();

        IEnumerable<FileDeletion> GetFileDeletions();

        IEnumerable<FileRenaming> GetFileRenamings();

        InstallationInfo? GetInstallation(int installationId);

        IEnumerable<InstallationInfo> GetInstallations();
    }
}