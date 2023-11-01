using System;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands.Monitor
{
    internal class MonitorCommand : Command
    {
        private readonly string? directory;
        private readonly string? programName;
        public CancellationToken cancellationToken;

        internal MonitorCommand(string? directory, string? programName, CancellationToken cancellationToken)
        {
            this.directory = directory;
            this.programName = programName;
            this.cancellationToken = cancellationToken;
        }

        protected override async Task ExecuteAsync(IServiceProvider serviceProvider)
        {
            await InstallationsMonitor.MonitorAsync(
                this.directory, this.programName, this.cancellationToken);
        }
    }
}