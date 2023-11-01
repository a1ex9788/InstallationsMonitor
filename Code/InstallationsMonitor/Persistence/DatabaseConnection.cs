using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InstallationsMonitor.Persistence
{
    internal class DatabaseConnection : IDisposable
    {
        private readonly AppDbContext appDbContext;

        // A semaphore is needed to avoid concurrency problems among different monitoring actions in
        // different directories.
        private readonly SemaphoreSlim semaphoreSlim;

        public DatabaseConnection(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.semaphoreSlim = new SemaphoreSlim(1);
        }

        public void Dispose()
        {
            this.semaphoreSlim.Dispose();
        }

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

        internal void CreateFileOperation(FileOperation fileOperation)
        {
            this.Lock();

            this.appDbContext.FileOperations.Add(fileOperation);
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

        internal IEnumerable<FileOperation> GetFileOperations()
        {
            this.Lock();

            DbSet<FileOperation> fileOperations = this.appDbContext.FileOperations;

            this.Unlock();

            return fileOperations;
        }

        private void Lock()
        {
            // WaitAsync may have better performance but it can not be used because creation actions
            // are called from events that do not accept asynchronous delegates.
            this.semaphoreSlim.Wait();
        }

        private void Unlock()
        {
            this.semaphoreSlim.Release();
        }
    }
}