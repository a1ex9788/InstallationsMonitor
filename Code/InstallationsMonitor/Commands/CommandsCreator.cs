using InstallationsMonitor.Commands.Monitor;
using System;
using System.Threading;

namespace InstallationsMonitor.Commands
{
    // This class creates hooks to override at tests.
    internal static class CommandsCreator
    {
        internal static Func<string?, string?, CancellationToken, ICommand>
            CreateMonitorCommandImplementation = (d, pn, ct) => new MonitorCommand(d, pn, ct);

        internal static ICommand CreateMonitorCommand(
            string? directory, string? programName, CancellationToken cancellationToken)
        {
            return CreateMonitorCommandImplementation(directory, programName, cancellationToken);
        }
    }
}