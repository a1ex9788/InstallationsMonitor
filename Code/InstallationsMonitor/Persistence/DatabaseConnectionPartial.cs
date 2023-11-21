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

            this.appDbContext.Installations.Add(installation);
            this.appDbContext.SaveChanges();

            int id = this.appDbContext.Installations
                .First(i => i.ProgramName == installation.ProgramName
                    && i.DateTime == installation.DateTime).Id;

            this.Unlock();

            return id;
        }

        public void CreateFileChange(FileChange fileChange)
        {
            this.Lock();

            this.appDbContext.FileChanges.Add(fileChange);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        public void CreateFileCreation(FileCreation fileCreation)
        {
            this.Lock();

            this.appDbContext.FileCreations.Add(fileCreation);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        public void CreateFileDeletion(FileDeletion fileDeletion)
        {
            this.Lock();

            this.appDbContext.FileDeletions.Add(fileDeletion);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        public void CreateFileRenaming(FileRenaming fileRenaming)
        {
            this.Lock();

            this.appDbContext.FileRenamings.Add(fileRenaming);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        public IEnumerable<Installation> GetInstallations()
        {
            this.Lock();

            DbSet<Installation> installations = this.appDbContext.Installations;

            this.Unlock();

            return installations;
        }

        public Installation? GetInstallation(int installationId)
        {
            this.Lock();

            Installation? installation = this.appDbContext.Installations
                .SingleOrDefault(i => i.Id == installationId);

            this.Unlock();

            return installation;
        }

        public IEnumerable<FileChange> GetFileChanges()
        {
            this.Lock();

            DbSet<FileChange> fileChanges = this.appDbContext.FileChanges;

            this.Unlock();

            return fileChanges;
        }

        public IEnumerable<FileCreation> GetFileCreations()
        {
            this.Lock();

            DbSet<FileCreation> fileCreations = this.appDbContext.FileCreations;

            this.Unlock();

            return fileCreations;
        }

        public IEnumerable<FileDeletion> GetFileDeletions()
        {
            this.Lock();

            DbSet<FileDeletion> fileDeletions = this.appDbContext.FileDeletions;

            this.Unlock();

            return fileDeletions;
        }

        public IEnumerable<FileRenaming> GetFileRenamings()
        {
            this.Lock();

            DbSet<FileRenaming> fileRenamings = this.appDbContext.FileRenamings;

            this.Unlock();

            return fileRenamings;
        }

        public void RemoveInstallation(int installationId)
        {
            this.Lock();

            Installation? installation = this.appDbContext.Installations
                .SingleOrDefault(i => i.Id == installationId);

            if (installation is not null)
            {
                this.appDbContext.Installations.Remove(installation);
                this.appDbContext.SaveChanges();
            }

            this.Unlock();
        }

        public void RemoveFileOperations(int installationId)
        {
            this.Lock();

            IList<FileChange> fileChangesToRemove =
                this.appDbContext.FileChanges.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileChange fileChange in fileChangesToRemove)
            {
                this.appDbContext.FileChanges.Remove(fileChange);
            }

            IList<FileCreation> fileCreationsToRemove =
                this.appDbContext.FileCreations.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileCreation fileCreation in fileCreationsToRemove)
            {
                this.appDbContext.FileCreations.Remove(fileCreation);
            }

            IList<FileDeletion> fileDeletionsToRemove =
                this.appDbContext.FileDeletions.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileDeletion fileDeletion in fileDeletionsToRemove)
            {
                this.appDbContext.FileDeletions.Remove(fileDeletion);
            }

            IList<FileRenaming> fileRenamingsToRemove =
                this.appDbContext.FileRenamings.Where(fc => fc.InstallationId == installationId).ToList();

            foreach (FileRenaming fileRenaming in fileRenamingsToRemove)
            {
                this.appDbContext.FileRenamings.Remove(fileRenaming);
            }

            this.appDbContext.SaveChanges();

            this.Unlock();
        }
    }
}