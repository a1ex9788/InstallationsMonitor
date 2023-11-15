using InstallationsMonitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstallationsMonitor.Persistence
{
    internal partial class DatabaseConnection : IDisposable
    {
        internal int CreateInstallation(Installation installation)
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

        internal void CreateFileChange(FileChange fileChange)
        {
            this.Lock();

            this.appDbContext.FileChanges.Add(fileChange);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        internal void CreateFileCreation(FileCreation fileCreation)
        {
            this.Lock();

            this.appDbContext.FileCreations.Add(fileCreation);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        internal void CreateFileDeletion(FileDeletion fileDeletion)
        {
            this.Lock();

            this.appDbContext.FileDeletions.Add(fileDeletion);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        internal void CreateFileRenaming(FileRenaming fileRenaming)
        {
            this.Lock();

            this.appDbContext.FileRenamings.Add(fileRenaming);
            this.appDbContext.SaveChanges();

            this.Unlock();
        }

        internal IEnumerable<Installation> GetInstallations()
        {
            this.Lock();

            DbSet<Installation> installations = this.appDbContext.Installations;

            this.Unlock();

            return installations;
        }

        internal IEnumerable<FileChange> GetFileChanges()
        {
            this.Lock();

            DbSet<FileChange> fileChanges = this.appDbContext.FileChanges;

            this.Unlock();

            return fileChanges;
        }

        internal IEnumerable<FileCreation> GetFileCreations()
        {
            this.Lock();

            DbSet<FileCreation> fileCreations = this.appDbContext.FileCreations;

            this.Unlock();

            return fileCreations;
        }

        internal IEnumerable<FileDeletion> GetFileDeletions()
        {
            this.Lock();

            DbSet<FileDeletion> fileDeletions = this.appDbContext.FileDeletions;

            this.Unlock();

            return fileDeletions;
        }

        internal IEnumerable<FileRenaming> GetFileRenamings()
        {
            this.Lock();

            DbSet<FileRenaming> fileRenamings = this.appDbContext.FileRenamings;

            this.Unlock();

            return fileRenamings;
        }
    }
}