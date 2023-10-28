using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using System;
using System.Collections.Generic;

namespace InstallationsMonitor.Persistence
{
    internal class DatabaseConnection : IDisposable
    {
        private readonly AppDbContext appDbContext;

        public DatabaseConnection(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void Dispose()
        {
            this.appDbContext.Dispose();
        }

        internal void CreateInstallation(Installation installation)
        {
            this.appDbContext.Installations.Add(installation);

            this.appDbContext.SaveChanges();
        }

        internal void CreateFileOperation(FileOperation fileOperation)
        {
            this.appDbContext.FileOperations.Add(fileOperation);

            this.appDbContext.SaveChanges();
        }

        internal IEnumerable<Installation> GetInstallations()
        {
            return this.appDbContext.Installations;
        }

        internal IEnumerable<FileOperation> GetFileOperations()
        {
            return this.appDbContext.FileOperations;
        }
    }
}