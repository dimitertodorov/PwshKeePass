using PwshKeePass.Commands.Password;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class GetKeePassPasswordProfileTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public GetKeePassPasswordProfileTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestGetKeePassPasswordProfile()
        {
            var unused = _fixture.PasswordService.NewKeePassPasswordProfile(true, true, false, false, false, false,
                false, false, false, false, null, 45, "TestGetKeePassPasswordProfile");
            var cmdlet = new GetKeePassPasswordProfile
            {
                CommandRuntime = commandRuntimeMock,
                KeePassProfile = _fixture.KeePassProfile,
                PasswordProfileName = "TestGetKeePassPasswordProfile"
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestGetKeePassPasswordProfileNonExistent()
        {
            var cmdlet = new GetKeePassPasswordProfile
            {
                CommandRuntime = commandRuntimeMock,
                KeePassProfile = _fixture.KeePassProfile,
                PasswordProfileName = "TestGetKeePassPasswordProfileNonExistent"
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.Empty(commandRuntimeMock.OutputPipeline);
        }
    }
}