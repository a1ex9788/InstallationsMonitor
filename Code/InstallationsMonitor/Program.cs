using InstallationsMonitor.Logic.Contracts;
using InstallationsMonitor.ServiceProviders.Delete;
using InstallationsMonitor.ServiceProviders.Installation;
using InstallationsMonitor.ServiceProviders.Installations;
using InstallationsMonitor.ServiceProviders.Monitor;
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

            DefineDeleteCommand(commandLineApplication);
            DefineInstallationCommand(commandLineApplication);
            DefineInstallationsCommand(commandLineApplication);
            DefineMonitorCommand(commandLineApplication);

            return commandLineApplication.Execute(args);
        }

        private static void DefineDeleteCommand(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command(
                "delete",
                command =>
                {
                    CommandOption installationIdCommandOption = command.Option(
                        "-i",
                        "The identifier of the installation to delete.",
                        CommandOptionType.SingleValue).IsRequired();

                    command.OnExecuteAsync(async ct =>
                    {
                        IServiceProvider serviceProvider =
                            new DeleteCommandServiceProvider(ct, Settings.GetDatabaseFullName());
                        IDeleteCommand deleteCommand = serviceProvider
                            .GetRequiredService<IDeleteCommand>();

                        bool isInt = int.TryParse(
                            installationIdCommandOption.Value(), out int installationIdentifier);

                        if (!isInt)
                        {
                            // Use of McMaster for this is avoided since it throws exception.
                            await Console.Error.WriteLineAsync("The -i field must be an integer.");

                            return;
                        }

                        deleteCommand.Execute(installationIdentifier);
                    });
                });
        }

        private static void DefineInstallationCommand(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command(
                "installation",
                command =>
                {
                    CommandOption installationIdCommandOption = command.Option(
                        "-i",
                        "The identifier of the installation to show its file operations.",
                        CommandOptionType.SingleValue).IsRequired();

                    command.OnExecuteAsync(async ct =>
                    {
                        IServiceProvider serviceProvider =
                            new InstallationCommandServiceProvider(ct, Settings.GetDatabaseFullName());
                        IInstallationCommand installationCommand = serviceProvider
                            .GetRequiredService<IInstallationCommand>();

                        bool isInt = int.TryParse(
                            installationIdCommandOption.Value(), out int installationIdentifier);

                        if (!isInt)
                        {
                            // Use of McMaster for this is avoided since it throws exception.
                            await Console.Error.WriteLineAsync("The -i field must be an integer.");

                            return;
                        }

                        installationCommand.Execute(installationIdentifier);
                    });
                });
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
    }
}