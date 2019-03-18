using PwshKeePass.Commands.Database;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class GetKeePassDatabaseConfigurationTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public GetKeePassDatabaseConfigurationTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestGetKeePassDatabaseConfiguration()
        {
            var cmdlet = new NewKeePassDatabaseConfiguration
            {
                CommandRuntime = commandRuntimeMock,
                KeyPath = _fixture.KeyPath,
                MasterKey = _fixture.MasterKey.ConvertToSecureString(),
                DatabasePath = _fixture.DatabasePath,
                DatabaseProfileName = "TEST_GET",
                Force = true
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.Empty(commandRuntimeMock.OutputPipeline);
            commandRuntimeMock.OutputPipeline.Clear();

            var getCmdlet = new GetKeePassDatabaseConfiguration
            {
                CommandRuntime = commandRuntimeMock,
                DatabaseProfileName = "TEST_GET"
            };
            getCmdlet.InvokeBeginProcessing();
            getCmdlet.ExecuteCmdlet();
            getCmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestGetKeePassDatabaseConfigurationAll()
        {
            var cmdlet = new NewKeePassDatabaseConfiguration
            {
                CommandRuntime = commandRuntimeMock,
                KeyPath = _fixture.KeyPath,
                MasterKey = _fixture.MasterKey.ConvertToSecureString(),
                DatabasePath = _fixture.DatabasePath,
                DatabaseProfileName = "TEST_GET_ALL",
                Force = true
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.Empty(commandRuntimeMock.OutputPipeline);
            commandRuntimeMock.OutputPipeline.Clear();

            var getCmdlet = new GetKeePassDatabaseConfiguration
            {
                CommandRuntime = commandRuntimeMock
            };
            getCmdlet.InvokeBeginProcessing();
            getCmdlet.ExecuteCmdlet();
            getCmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }
    }
}