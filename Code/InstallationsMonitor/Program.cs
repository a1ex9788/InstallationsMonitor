﻿using InstallationsMonitor.Commands.Monitor;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InstallationsMonitor
{
    internal class Program
    {
        internal static int Main(string[] args)
        {
            using CommandLineApplication commandLineApplication = new CommandLineApplication()
            {
                Name = "installationsMonitor",
            };

            commandLineApplication.HelpOption();

            DefineMonitorCommand(commandLineApplication);

            return commandLineApplication.Execute(args);
        }

        private static void DefineMonitorCommand(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command(
                "monitor",
                command =>
                {
                    CommandOption directoryCommandOption = command.Option(
                        "-d",
                        "The directory to monitor.",
                        CommandOptionType.SingleValue);

                    CommandOption programNameCommandOption = command.Option(
                        "-p",
                        "The name of the program of the installation to monitor.",
                        CommandOptionType.SingleValue);

                    command.OnExecuteAsync(async ct =>
                    {
                        IServiceProvider serviceProvider = new MonitorCommandServiceProvider(ct);
                        IMonitorCommand monitorCommand = serviceProvider
                            .GetRequiredService<IMonitorCommand>();

                        await monitorCommand.ExecuteAsync(
                            directoryCommandOption.Value(), programNameCommandOption.Value());
                    });
                });
        }
    }
}