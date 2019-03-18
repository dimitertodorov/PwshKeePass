using System.Management.Automation;
using KeePassLib;
using PwshKeePass.Commands.Group;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class RemoveKeePassGroupTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public RemoveKeePassGroupTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestRemoveKeePassGroup()
        {
            var group = _fixture.GroupService.CreateGroup("SampleDb", "TestGroupRemove", PwIcon.Star);
            var cmdlet = new RemoveKeePassGroup()
            {
                CommandRuntime = commandRuntimeMock,
                Connection = _fixture.GetConnection(),
                KeePassGroup = new PSObject(group),
                Force = true
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            var recycledGroup = _fixture.GroupService.GetGroup("SampleDb/Recycle Bin/TestGroupRemove");
            Assert.Equal(PwIcon.Star, recycledGroup.IconId);
        }

        [Fact]
        public void TestRemoveKeePassGroupNoRecycle()
        {
            var group = _fixture.GroupService.CreateGroup("SampleDb", "TestGroupRemoveNoRecycle", PwIcon.Star);
            var cmdlet = new RemoveKeePassGroup()
            {
                CommandRuntime = commandRuntimeMock,
                Connection = _fixture.GetConnection(),
                KeePassGroup = new PSObject(group),
                Force = true,
                NoRecycle = true
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.Throws<PSArgumentException>(() =>
                _fixture.GroupService.GetGroup("SampleDb/Recycle Bin/TestGroupRemoveNoRecycle"));
        }
    }
}