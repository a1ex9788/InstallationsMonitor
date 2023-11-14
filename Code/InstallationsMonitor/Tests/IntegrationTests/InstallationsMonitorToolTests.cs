using CliWrap;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}