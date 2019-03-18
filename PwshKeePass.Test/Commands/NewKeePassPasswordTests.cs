using System.Management.Automation;
using PwshKeePass.Commands.Password;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class NewKeePassPasswordTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public NewKeePassPasswordTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestNewKeePassPassword()
        {
            var cmdlet = new NewKeePassPassword
            {
                CommandRuntime = commandRuntimeMock,
                KeePassProfile = _fixture.KeePassProfile,
                UpperCase = true,
                LowerCase = true,
                Digits = true,
                SpecialCharacters = true,
                Minus = true,
                UnderScore = true,
                Space = true,
                Brackets = true,
                ExcludeLookALike = true,
                Length = 20,
                SaveAs = "TestNewKeePassPassword"
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestNewKeePassPasswordBadOptions()
        {
            var cmdlet = new NewKeePassPassword
            {
                CommandRuntime = commandRuntimeMock,
                KeePassProfile = _fixture.KeePassProfile,
                UpperCase = false,
                LowerCase = false,
                Digits = true,
                SpecialCharacters = false,
                Minus = false,
                UnderScore = false,
                Space = false,
                Brackets = false,
                ExcludeLookALike = true,
                Length = 25,
                NoRepeatingCharacters = true,
                ExcludeCharacters = "0123",
                SaveAs = "TestNewKeePassPasswordBadOptions"
            };
            cmdlet.InvokeBeginProcessing();
            Assert.Throws<PSArgumentException>(() => cmdlet.ExecuteCmdlet());
            cmdlet.InvokeEndProcessing();
        }

        [Fact]
        public void TestNewKeePassPasswordFromProfile()
        {
            var profile = _fixture.PasswordService.NewKeePassPasswordProfile(true, true, false, false, false, false,
                false, false, false, false, null, 45, "TestNewKeePassPasswordFromProfile");
            var cmdlet = new NewKeePassPassword
            {
                CommandRuntime = commandRuntimeMock,
                KeePassProfile = _fixture.KeePassProfile,
                PasswordProfileName = "TestNewKeePassPasswordFromProfile"
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestNewKeePassPasswordFromProfileNonExistent()
        {
            var cmdlet = new NewKeePassPassword
            {
                CommandRuntime = commandRuntimeMock,
                KeePassProfile = _fixture.KeePassProfile,
                PasswordProfileName = "TestGetKeePassPasswordProfileNonExistent"
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.ErrorStream);
        }
    }
}