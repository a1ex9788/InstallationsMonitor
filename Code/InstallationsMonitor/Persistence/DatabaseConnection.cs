﻿using InstallationsMonitor.Persistence.Contracts;
using System;
using System.Threading;

namespace InstallationsMonitor.Persistence
{
    public sealed partial class DatabaseConnection : IDatabaseConnection, IDisposable
    {
        private readonly DatabaseContext databaseContext;

        // A semaphore is needed to avoid concurrency problems among different monitoring actions in
        // different directories.
        private readonly SemaphoreSlim semaphoreSlim;

        public DatabaseConnection(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
            this.semaphoreSlim = new SemaphoreSlim(1);
        }

        public void Dispose()
        {
            this.semaphoreSlim.Dispose();
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