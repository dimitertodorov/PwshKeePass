using PwshKeePass.Commands.Entry;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class GetKeePassEntryTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public GetKeePassEntryTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestGetEntryBase()
        {
            var cmdlet = new GetKeePassEntry
            {
                CommandRuntime = commandRuntimeMock, Connection = _fixture.GetConnection()
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestGetEntryWithPath()
        {
            var cmdlet = new GetKeePassEntry
            {
                CommandRuntime = commandRuntimeMock, Connection = _fixture.GetConnection()
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.KeePassEntryGroupPath = "SampleDb";
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }
    }
}