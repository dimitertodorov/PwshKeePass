using PwshKeePass.Commands.Database;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class NewKeePassDatabaseConfigurationTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public NewKeePassDatabaseConfigurationTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestNewKeePassDatabaseConfiguration()
        {
            var cmdlet = new NewKeePassDatabaseConfiguration
            {
                CommandRuntime = commandRuntimeMock
            };
            cmdlet.KeyPath = _fixture.KeyPath;
            cmdlet.MasterKey = _fixture.MasterKey.ConvertToSecureString();
            cmdlet.DatabasePath = _fixture.DatabasePath;
            cmdlet.DatabaseProfileName = _fixture.DatabaseProfileName;
            cmdlet.Force = true;
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.Empty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestNewKeePassDatabaseConfigurationPassThru()
        {
            var cmdlet = new NewKeePassDatabaseConfiguration
            {
                CommandRuntime = commandRuntimeMock
            };
            cmdlet.KeyPath = _fixture.KeyPath;
            cmdlet.MasterKey = _fixture.MasterKey.ConvertToSecureString();
            cmdlet.DatabasePath = _fixture.DatabasePath;
            cmdlet.DatabaseProfileName = _fixture.DatabaseProfileName;
            cmdlet.Force = true;
            cmdlet.Default = true;
            cmdlet.PassThru = true;
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }
    }
}