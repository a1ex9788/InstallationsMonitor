using InstallationsMonitor.Domain;
using System.Collections.Generic;

namespace InstallationsMonitor.Persistence.Contracts
{
    public interface IDatabaseConnectionEntitiesObtainer
    {
        IEnumerable<Installation> GetInstallations();

        Installation? GetInstallation(int installationId);

        IEnumerable<FileChange> GetFileChanges();

        IEnumerable<FileCreation> GetFileCreations();

        IEnumerable<FileDeletion> GetFileDeletions();

        IEnumerable<FileRenaming> GetFileRenamings();
    }
}