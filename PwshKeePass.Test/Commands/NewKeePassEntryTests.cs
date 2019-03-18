using System.Linq;
using System.Management.Automation;
using PwshKeePass.Commands.Entry;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Model;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class NewKeePassEntryTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private readonly MockCommandRuntime _commandRuntimeMock;

        public NewKeePassEntryTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            _commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestCreateEntryBase()
        {
            var cmdlet = new NewKeePassEntry();
            cmdlet.CommandRuntime = _commandRuntimeMock;
            cmdlet.Connection = _fixture.GetConnection();
            cmdlet.KeePassPassword = "asdasd";
            cmdlet.KeePassEntryGroupPath = "SampleDb";
            cmdlet.Title = "TEST123";
            cmdlet.UserName = "TEST1235";
            cmdlet.PassThru = true;
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(_commandRuntimeMock.OutputPipeline);
            var ent = (PSKeePassEntry) _commandRuntimeMock.OutputPipeline.First();
            Assert.NotNull(ent);
            var pass = ent.Password.ConvertToUnsecureString();
            Assert.Equal("asdasd",pass);
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