using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Tests.Utilities
{
    internal class FakeDatabaseConnection : IDatabaseConnection
    {
        private readonly IList<InstallationInfo> installations = new List<InstallationInfo>();
        private readonly IList<FileChange> fileChanges = new List<FileChange>();
        private readonly IList<FileCreation> fileCreations = new List<FileCreation>();
        private readonly IList<FileDeletion> fileDeletions = new List<FileDeletion>();
        private readonly IList<FileRenaming> fileRenamings = new List<FileRenaming>();

        public void CreateFileChange(FileChange fileChange)
        {
            this.fileChanges.Add(fileChange);
        }

        public void CreateFileCreation(FileCreation fileCreation)
        {
            this.fileCreations.Add(fileCreation);
        }

        public void CreateFileDeletion(FileDeletion fileDeletion)
        {
            this.fileDeletions.Add(fileDeletion);
        }

        public void CreateFileRenaming(FileRenaming fileRenaming)
        {
            this.fileRenamings.Add(fileRenaming);
        }

        public int CreateInstallation(InstallationInfo installation)
        {
            installation.Id = this.installations.Count + 1;

            this.installations.Add(installation);

            return installation.Id;
        }

        public void DeleteFileOperations(int installationId)
        {
            foreach (FileChange fileChange in
                this.fileChanges.Where(fc => fc.InstallationId == installationId).ToList())
            {
                this.fileChanges.Remove(fileChange);
            }

            foreach (FileCreation fileCreation in
                this.fileCreations.Where(fc => fc.InstallationId == installationId).ToList())
            {
                this.fileCreations.Remove(fileCreation);
            }

            foreach (FileDeletion fileDeletion in
                this.fileDeletions.Where(fc => fc.InstallationId == installationId).ToList())
            {
                this.fileDeletions.Remove(fileDeletion);
            }

            foreach (FileRenaming fileRenaming in
                this.fileRenamings.Where(fc => fc.InstallationId == installationId).ToList())
            {
                this.fileRenamings.Remove(fileRenaming);
            }
        }

        public void DeleteInstallation(int installationId)
        {
            InstallationInfo? installation = this.GetInstallation(installationId);

            if (installation is not null)
            {
                this.installations.Remove(installation);
            }
        }

        public IEnumerable<FileChange> GetFileChanges()
        {
            return this.fileChanges;
        }

        public IEnumerable<FileChange> GetFileChanges(int installationId)
        {
            return this.fileChanges.Where(fc => fc.InstallationId == installationId);
        }

        public IEnumerable<FileCreation> GetFileCreations()
        {
            return this.fileCreations;
        }

        public IEnumerable<FileCreation> GetFileCreations(int installationId)
        {
            return this.fileCreations.Where(fc => fc.InstallationId == installationId);
        }

        public IEnumerable<FileDeletion> GetFileDeletions()
        {
            return this.fileDeletions;
        }

        public IEnumerable<FileDeletion> GetFileDeletions(int installationId)
        {
            return this.fileDeletions.Where(fd => fd.InstallationId == installationId);
        }

        public IEnumerable<FileRenaming> GetFileRenamings()
        {
            return this.fileRenamings;
        }

        public IEnumerable<FileRenaming> GetFileRenamings(int installationId)
        {
            return this.fileRenamings.Where(fr => fr.InstallationId == installationId);
        }

        public InstallationInfo? GetInstallation(int installationId)
        {
            return this.installations.SingleOrDefault(i => i.Id == installationId);
        }

        public IEnumerable<InstallationInfo> GetInstallations()
        {
            return this.installations;
        }
    }
}