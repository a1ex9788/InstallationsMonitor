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
        public int CreateInstallation(Installation installation)
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

        public IEnumerable<Installation> GetInstallations()
        {
            this.Lock();

            DbSet<Installation> installations = this.databaseContext.Installations;

            this.Unlock();

            return installations;
        }

        public Installation? GetInstallation(int installationId)
        {
            this.Lock();

            Installation? installation = this.databaseContext.Installations
                .SingleOrDefault(i => i.Id == installationId);

            this.Unlock();

            return installation;
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

        public void RemoveInstallation(int installationId)
        {
            this.Lock();

            Installation? installation = this.databaseContext.Installations
                .SingleOrDefault(i => i.Id == installationId);

            if (installation is not null)
            {
                this.databaseContext.Installations.Remove(installation);
                this.databaseContext.SaveChanges();
            }

            this.Unlock();
        }

        public void RemoveFileOperations(int installationId)
        {
            this.Lock();

            IList<FileChange> fileChangesToRemove =
                this.databaseContext.FileChanges.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileChange fileChange in fileChangesToRemove)
            {
                this.databaseContext.FileChanges.Remove(fileChange);
            }

            IList<FileCreation> fileCreationsToRemove =
                this.databaseContext.FileCreations.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileCreation fileCreation in fileCreationsToRemove)
            {
                this.databaseContext.FileCreations.Remove(fileCreation);
            }

            IList<FileDeletion> fileDeletionsToRemove =
                this.databaseContext.FileDeletions.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileDeletion fileDeletion in fileDeletionsToRemove)
            {
                this.databaseContext.FileDeletions.Remove(fileDeletion);
            }

            IList<FileRenaming> fileRenamingsToRemove =
                this.databaseContext.FileRenamings.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileRenaming fileRenaming in fileRenamingsToRemove)
            {
                this.databaseContext.FileRenamings.Remove(fileRenaming);
            }

            this.databaseContext.SaveChanges();

            this.Unlock();
        }
    }
}