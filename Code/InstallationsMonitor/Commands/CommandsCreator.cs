using InstallationsMonitor.Commands.Monitor;
using System;
using System.Threading;

namespace InstallationsMonitor.Commands
{
    // This class creates hooks to override at tests.
    internal static class CommandsCreator
    {
        internal static Func<string?, CancellationToken, ICommand> CreateMonitorCommandImplementation =
            (d, ct) => new MonitorCommand(d, ct);

        internal static ICommand CreateMonitorCommand(
            string? directory, CancellationToken cancellationToken)
        {
            return CreateMonitorCommandImplementation(directory, cancellationToken);
        }
    }
}