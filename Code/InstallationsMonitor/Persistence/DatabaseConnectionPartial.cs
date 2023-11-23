using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstallationsMonitor.Persistence
{
    public sealed partial class DatabaseConnection : IDatabaseConnection, IDisposable
    {
        public void CreateFileChange(FileChange fileChange)
        {
            this.Lock();

            this.databaseContext.FileChanges.Add(fileChange);
            this.databaseContext.SaveChanges();

            this.Unlock();
        }

        public void CreateFileCreation(FileCreation fileCreation)
        {
            this.Lock();

            this.databaseContext.FileCreations.Add(fileCreation);
            this.databaseContext.SaveChanges();

            this.Unlock();
        }

        public void CreateFileDeletion(FileDeletion fileDeletion)
        {
            this.Lock();

            this.databaseContext.FileDeletions.Add(fileDeletion);
            this.databaseContext.SaveChanges();

            this.Unlock();
        }

        public void CreateFileRenaming(FileRenaming fileRenaming)
        {
            this.Lock();

            this.databaseContext.FileRenamings.Add(fileRenaming);
            this.databaseContext.SaveChanges();

            this.Unlock();
        }

        public int CreateInstallation(InstallationInfo installation)
        {
            this.Lock();

            this.databaseContext.Installations.Add(installation);
            this.databaseContext.SaveChanges();

            int id = this.databaseContext.Installations
                .Single(i => i.ProgramName == installation.ProgramName
                    && i.DateTime == installation.DateTime).Id;

            this.Unlock();

            return id;
        }

        public void DeleteInstallation(int installationId)
        {
            this.Lock();

            InstallationInfo? installation = this.databaseContext.Installations
                .SingleOrDefault(i => i.Id == installationId);

            if (installation is not null)
            {
                this.databaseContext.Installations.Remove(installation);
                this.databaseContext.SaveChanges();
            }

            this.Unlock();
        }

        public void DeleteFileOperations(int installationId)
        {
            this.Lock();

            IList<FileChange> fileChangesToDelete =
                this.databaseContext.FileChanges.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileChange fileChange in fileChangesToDelete)
            {
                this.databaseContext.FileChanges.Remove(fileChange);
            }

            IList<FileCreation> fileCreationsToDelete =
                this.databaseContext.FileCreations.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileCreation fileCreation in fileCreationsToDelete)
            {
                this.databaseContext.FileCreations.Remove(fileCreation);
            }

            IList<FileDeletion> fileDeletionsToDelete =
                this.databaseContext.FileDeletions.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileDeletion fileDeletion in fileDeletionsToDelete)
            {
                this.databaseContext.FileDeletions.Remove(fileDeletion);
            }

            IList<FileRenaming> fileRenamingsToDelete =
                this.databaseContext.FileRenamings.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileRenaming fileRenaming in fileRenamingsToDelete)
            {
                this.databaseContext.FileRenamings.Remove(fileRenaming);
            }

            this.databaseContext.SaveChanges();

            this.Unlock();
        }

        public IEnumerable<FileChange> GetFileChanges()
        {
            this.Lock();

            DbSet<FileChange> fileChanges = this.databaseContext.FileChanges;

            this.Unlock();

            return fileChanges;
        }

        public IEnumerable<FileCreation> GetFileCreations()
        {
            this.Lock();

            DbSet<FileCreation> fileCreations = this.databaseContext.FileCreations;

            this.Unlock();

            return fileCreations;
        }

        public IEnumerable<FileDeletion> GetFileDeletions()
        {
            this.Lock();

            DbSet<FileDeletion> fileDeletions = this.databaseContext.FileDeletions;

            this.Unlock();

            return fileDeletions;
        }

        public IEnumerable<FileRenaming> GetFileRenamings()
        {
            this.Lock();

            DbSet<FileRenaming> fileRenamings = this.databaseContext.FileRenamings;

            this.Unlock();

            return fileRenamings;
        }

        public InstallationInfo? GetInstallation(int installationId)
        {
            this.Lock();

            InstallationInfo? installation = this.databaseContext.Installations
                .SingleOrDefault(i => i.Id == installationId);

            this.Unlock();

            return installation;
        }

        public IEnumerable<InstallationInfo> GetInstallations()
        {
            this.Lock();

            DbSet<InstallationInfo> installations = this.databaseContext.Installations;

            this.Unlock();

            return installations;
        }
    }
}