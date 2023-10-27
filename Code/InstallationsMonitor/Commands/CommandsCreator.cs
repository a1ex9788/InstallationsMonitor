﻿using InstallationsMonitor.Commands.Monitor;
using System;
using System.Threading;

namespace InstallationsMonitor.Commands
{
    // This class creates hooks to override at tests.
    internal static class CommandsCreator
    {
        internal static Func<string?, CancellationToken, MonitorCommand> CreateMonitorcommandImplementation =
            (directory, cancellationToken) => new MonitorCommand(directory, cancellationToken);

        internal static MonitorCommand CreateMonitorCommand(
            string? directory, CancellationToken cancellationToken)
        {
            return CreateMonitorcommandImplementation(directory, cancellationToken);
        }
    }
}