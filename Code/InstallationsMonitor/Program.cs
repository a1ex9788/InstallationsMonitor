﻿using InstallationsMonitor.Logic.Commands.Installations;
using InstallationsMonitor.Logic.Commands.Monitor;
using InstallationsMonitor.Logic.Commands.Remove;
using InstallationsMonitor.ServiceProviders.Installations;
using InstallationsMonitor.ServiceProviders.Monitor;
using InstallationsMonitor.ServiceProviders.Remove;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace InstallationsMonitor
{
    public class Program
    {
        public static int Main(string[] args)
        {
            using CommandLineApplication commandLineApplication = new CommandLineApplication()
            {
                Name = "installationsMonitor",
            };

            commandLineApplication.HelpOption();

            DefineInstallationsCommand(commandLineApplication);
            DefineMonitorCommand(commandLineApplication);
            DefineRemoveCommand(commandLineApplication);

            return commandLineApplication.Execute(args);
        }

        private static void DefineInstallationsCommand(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command(
                "installations",
                command =>
                {
                    command.OnExecuteAsync(ct =>
                    {
                        IServiceProvider serviceProvider =
                            new InstallationsCommandServiceProvider(ct, Settings.GetDatabaseFullName());
                        IInstallationsCommand installationsCommand = serviceProvider
                            .GetRequiredService<IInstallationsCommand>();

                        installationsCommand.Execute();

                        return Task.CompletedTask;
                    });
                });
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
                        IServiceProvider serviceProvider =
                            new MonitorCommandServiceProvider(ct, Settings.GetDatabaseFullName());
                        IMonitorCommand monitorCommand = serviceProvider
                            .GetRequiredService<IMonitorCommand>();

                        await monitorCommand.ExecuteAsync(
                            directoryCommandOption.Value(), programNameCommandOption.Value());
                    });
                });
        }

        private static void DefineRemoveCommand(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command(
                "remove",
                command =>
                {
                    CommandOption installationIdCommandOption = command.Option(
                        "-i",
                        "The identifier of the installation to remove.",
                        CommandOptionType.SingleValue).IsRequired();

                    command.OnExecuteAsync(async ct =>
                    {
                        IServiceProvider serviceProvider =
                            new RemoveCommandServiceProvider(ct, Settings.GetDatabaseFullName());
                        IRemoveCommand removeCommand = serviceProvider
                            .GetRequiredService<IRemoveCommand>();

                        bool isInt = int.TryParse(
                            installationIdCommandOption.Value(), out int installationIdentifier);

                        if (!isInt)
                        {
                            // Use of McMaster for this is avoided since it throws exception.
                            await Console.Error.WriteLineAsync("The -i field must be an integer.");

                            return;
                        }

                        removeCommand.Execute(installationIdentifier);
                    });
                });
        }
    }
}