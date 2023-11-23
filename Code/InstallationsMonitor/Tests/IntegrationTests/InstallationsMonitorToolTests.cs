using CliWrap;
using FluentAssertions;
using InstallationsMonitor.Logic.Contracts;
using InstallationsMonitor.ServiceProviders.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Text;
using System.Threading.Tasks;

namespace InstallationsMonitor.Tests.IntegrationTests
{
    [TestClass]
    public class InstallationsMonitorToolTests
    {
        [TestMethod]
        public async Task InstallationsMonitorTool_AbbreviatedName_ToolExecutedCorrectly()
        {
            // Arrange.
            StringBuilder stringBuilder = new StringBuilder();

            Command command = Cli.Wrap("im")
                .WithArguments("--help")
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stringBuilder));

            // Act.
            await command.ExecuteAsync();

            // Assert.
            stringBuilder.ToString().Should().Contain("Usage: installationsMonitor [command] [options]");
        }

        [TestMethod]
        public void InstallationsMonitorTool_UnexpectedError_ExecutionEndsWithoutException()
        {
            // Arrange.
            string[] args = new string[] { "monitor" };

            IMonitorCommand monitorCommand = Substitute.For<IMonitorCommand>();
            monitorCommand
                .ExecuteAsync(Arg.Any<string?>(), Arg.Any<string?>())
                .Returns(x =>
                {
                    throw new Exception("Unexpected error.");
                });

            CommandsServiceProvider.ExtraRegistrationsAction = sc => sc.AddSingleton(monitorCommand);

            // Act.
            Action action = () => Program.Main(args);

            // Assert.
            action.Should().NotThrow();
        }
    }
}