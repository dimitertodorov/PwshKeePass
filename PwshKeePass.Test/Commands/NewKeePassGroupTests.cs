using System.Management.Automation;
using KeePassLib;
using PwshKeePass.Commands.Group;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class NewKeePassGroupTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public NewKeePassGroupTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestNewKeePassGroup()
        {
            var cmdlet = new NewKeePassGroup
            {
                CommandRuntime = commandRuntimeMock,
                Connection = _fixture.GetConnection(),
                KeePassGroupParentPath = "SampleDb/General",
                KeePassGroupName = "TestGroupXUNIT1",
                IconName = PwIcon.Star,
                PassThru = true
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestNewKeePassGroupThrow()
        {
            var cmdlet = new NewKeePassGroup
            {
                CommandRuntime = commandRuntimeMock,
                Connection = _fixture.GetConnection(),
                KeePassGroupParentPath = "SampleDb/GeneralBADGROUP",
                KeePassGroupName = "TestGroupXUNIT2",
                IconName = PwIcon.Star,
                PassThru = true
            };
            cmdlet.InvokeBeginProcessing();
            Assert.Throws<PSArgumentException>(() => cmdlet.ExecuteCmdlet());
            cmdlet.InvokeEndProcessing();
            Assert.Empty(commandRuntimeMock.OutputPipeline);
        }
    }
}