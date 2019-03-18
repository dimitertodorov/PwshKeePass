using PwshKeePass.Commands.Group;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class GetKeePassGroupTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public GetKeePassGroupTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestGetKeePassGroup()
        {
            var cmdlet = new GetKeePassGroup
            {
                CommandRuntime = commandRuntimeMock, Connection = _fixture.GetConnection()
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestGetKeePassGroupWithPath()
        {
            var cmdlet = new GetKeePassGroup
            {
                CommandRuntime = commandRuntimeMock,
                Connection = _fixture.GetConnection(),
                FullPath = "SampleDb/General"
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }
    }
}