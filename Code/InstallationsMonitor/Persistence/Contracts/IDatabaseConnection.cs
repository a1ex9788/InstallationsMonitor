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

        IEnumerable<FileChange> GetFileChanges(int installationId);

        IEnumerable<FileCreation> GetFileCreations();

        IEnumerable<FileCreation> GetFileCreations(int installationId);

        IEnumerable<FileDeletion> GetFileDeletions();

        IEnumerable<FileDeletion> GetFileDeletions(int installationId);

        IEnumerable<FileRenaming> GetFileRenamings();

        IEnumerable<FileRenaming> GetFileRenamings(int installationId);

        InstallationInfo? GetInstallation(int installationId);

        IEnumerable<InstallationInfo> GetInstallations();
    }
}