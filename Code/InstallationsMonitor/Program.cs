using InstallationsMonitor.Commands;
using InstallationsMonitor.Commands.Monitor;
using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

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
                        "-d", "The directory to monitor.", CommandOptionType.SingleValue);

                    command.OnExecuteAsync(ct => Task.Run(() =>
                    {
                        MonitorCommand monitorCommand = CommandsCreator.CreateMonitorCommand(
                            directoryCommandOption.Value(), ct);

                        monitorCommand.Execute();
                    },
                    ct));
                });
        }
    }
}