using InstallationsMonitor.Commands.Monitor;
using InstallationsMonitor.Persistence;
using System;
using System.Threading;
using InstallationsMonitorClass = InstallationsMonitor.Commands.Monitor.InstallationsMonitor;

namespace InstallationsMonitor.Tests.Utilities
{
    internal class Get
    {
        internal Get()
        {
        }

        internal Get(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        private CancellationToken? cancellationToken;
        private readonly DatabaseOptions databaseOptions = DatabaseUtilities.DatabaseOptions;

        private readonly DatabaseConnection databaseConnection = DatabaseUtilities
            .GetTestDatabaseConnection();

        private DatabaseFilesChecker? databaseFilesChecker;
        private DirectoriesMonitor? directoriesMonitor;
        private InstallationsMonitorClass? installationsMonitor;

        internal CancellationToken? CancellationToken
        {
            get => this.cancellationToken;
            set => this.cancellationToken = value;
        }

        internal DatabaseOptions DatabaseOptions => this.databaseOptions;

        internal DatabaseConnection DatabaseConnection => this.databaseConnection;

        internal DatabaseFilesChecker DatabaseFilesChecker
        {
            get
            {
                this.databaseFilesChecker ??= new DatabaseFilesChecker(this.DatabaseOptions);

                return this.databaseFilesChecker;
            }
        }

        internal DirectoriesMonitor DirectoriesMonitor
        {
            get
            {
                this.directoriesMonitor ??= new DirectoriesMonitor(
                    this.DatabaseConnection, this.DatabaseFilesChecker);

                return this.directoriesMonitor;
            }
        }

        internal InstallationsMonitorClass InstallationsMonitor
        {
            get
            {
                if (this.CancellationToken is null)
                {
                    throw new InvalidOperationException(
                        $"Property '{typeof(Get).Name}.{nameof(this.CancellationToken)}' must be set.");
                }

                this.installationsMonitor ??= new InstallationsMonitorClass(
                    this.DirectoriesMonitor, this.DatabaseConnection, this.CancellationToken.Value);

                return this.installationsMonitor;
            }
        }
    }
}