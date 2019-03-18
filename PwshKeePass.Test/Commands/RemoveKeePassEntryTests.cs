using System.Management.Automation;
using PwshKeePass.Commands.Entry;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class RemoveKeePassEntryTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private readonly MockCommandRuntime _commandRuntimeMock;

        public RemoveKeePassEntryTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            _commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestRemoveFromPipeline()
        {
            var entry = _fixture.EntryService.CreateEntry("TestRemoveFromPipeline", "USER",
                "pass".ConvertToSecureString(), null, null, null, "SampleDb");
            _commandRuntimeMock.ResetPipelines();
            var removeCmdlet = new RemoveKeePassEntry
            {
                CommandRuntime = _commandRuntimeMock,
                DatabaseProfileName = _fixture.DatabaseProfileName,
                KeePassEntry = new PSObject(entry.KPEntry),
                NoRecycle = false,
                Force = false
            };
            removeCmdlet.InvokeBeginProcessing();
            removeCmdlet.ExecuteCmdlet();
            removeCmdlet.InvokeEndProcessing();
            var entries = _fixture.EntryService.GetEntry(null, "TestRemoveFromPipeline", null, "SampleDb/Recycle Bin");
            Assert.NotEmpty(entries);
        }

        [Fact]
        public void TestRemoveNoRecycle()
        {
            var entry = _fixture.EntryService.CreateEntry("TestRemoveFromPipeline", "USER",
                "pass".ConvertToSecureString(), null, null, null, "SampleDb");
            _commandRuntimeMock.ResetPipelines();
            var removeCmdlet = new RemoveKeePassEntry
            {
                CommandRuntime = _commandRuntimeMock,
                DatabaseProfileName = _fixture.DatabaseProfileName,
                KeePassEntry = new PSObject(entry.KPEntry),
                NoRecycle = true,
                Force = false
            };
            removeCmdlet.InvokeBeginProcessing();
            removeCmdlet.ExecuteCmdlet();
            removeCmdlet.InvokeEndProcessing();
            var entries = _fixture.EntryService.GetEntry(null, "TestRemoveFromPipeline", null, "SampleDb/Recycle Bin");
            Assert.Empty(entries);
        }
    }
}