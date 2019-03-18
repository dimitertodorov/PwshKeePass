using System.Linq;
using System.Management.Automation;
using KeePassLib;
using PwshKeePass.Commands.Group;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Model;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class UpdateKeePassGroupTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private readonly MockCommandRuntime _commandRuntimeMock;

        public UpdateKeePassGroupTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            _commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestUpdateKeePassGroup()
        {
            var group = _fixture.GroupService.CreateGroup("SampleDb", "TestGroupUpdateOne", PwIcon.Star);
            var cmdlet = new UpdateKeePassGroup()
            {
                CommandRuntime = _commandRuntimeMock,
                DatabaseProfileName = _fixture.DatabaseProfileName,
                KeePassGroup = new PSObject(group),
                KeePassParentGroupPath = "SampleDb/General",
                IconName = PwIcon.WorldComputer,
                PassThru = true
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(_commandRuntimeMock.OutputPipeline);
            Assert.IsType<PSKeePassGroup>(_commandRuntimeMock.OutputPipeline.First());
            var updatedGroup = (PSKeePassGroup) _commandRuntimeMock.OutputPipeline.First();
            Assert.Equal("SampleDb/General/TestGroupUpdateOne", updatedGroup.FullPath);
        }

        [Fact]
        public void TestUpdateKeePassGroupNewName()
        {
            var group = _fixture.GroupService.CreateGroup("SampleDb", "TestGroupUpdate2", PwIcon.Star);
            var cmdlet = new UpdateKeePassGroup()
            {
                CommandRuntime = _commandRuntimeMock,
                Connection = _fixture.GetConnection(),
                KeePassGroup = new PSObject(group),
                KeePassGroupName = "TestGroupUpdate2_NEW",
                IconName = PwIcon.WorldComputer,
                PassThru = false,
                Force = true
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.Empty(_commandRuntimeMock.OutputPipeline);
        }
    }
}