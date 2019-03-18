using System.Management.Automation;
using PwshKeePass.Commands.Entry;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class UpdateKeePassEntryTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private readonly MockCommandRuntime _commandRuntimeMock;

        public UpdateKeePassEntryTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            _commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestUpdateEntryBase()
        {
            var cmdlet = new UpdateKeePassEntry();
            var entries = _fixture.EntryService.GetEntry(null, "nested3", null, null);
            cmdlet.CommandRuntime = _commandRuntimeMock;
            cmdlet.Connection = _fixture.GetConnection();
            cmdlet.KeePassEntry = new PSObject(entries[0]);
            cmdlet.UserName = "UserUpdated";
            cmdlet.PassThru = true;
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(_commandRuntimeMock.OutputPipeline);
        }
    }
}