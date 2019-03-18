using System.IO;
using PwshKeePass.Commands.Database;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Test.Mocks;
using Xunit;

namespace PwshKeePass.Test.Commands
{
    [Collection("CmdletTests")]
    public class NewKeePassConnectionTests : IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        private MockCommandRuntime commandRuntimeMock;

        public NewKeePassConnectionTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
            commandRuntimeMock = new MockCommandRuntime();
        }

        [Fact]
        public void TestNewKeePassConnection()
        {
            var cmdlet = new NewKeePassConnection();
            cmdlet.CommandRuntime = commandRuntimeMock;
            cmdlet.DatabasePath = _fixture.DatabasePath;
            cmdlet.MasterKey = _fixture.MasterKey.ConvertToSecureString();
            cmdlet.KeyPath = _fixture.KeyPath;

            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestNewKeePassConnectionMasterKey()
        {
            var cmdlet = new NewKeePassConnection();
            cmdlet.CommandRuntime = commandRuntimeMock;
            cmdlet.DatabasePath =
                Path.GetFullPath(@"./PowerShell.Tests/Includes/AuthenticationDatabases/MasterKey.kdbx");
            cmdlet.MasterKey = "ATestPassWord".ConvertToSecureString();

            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }

        [Fact]
        public void TestNewKeePassConnectionKeyAndMaster()
        {
            var cmdlet = new NewKeePassConnection
            {
                CommandRuntime = commandRuntimeMock,
                DatabasePath = Path.GetFullPath(@"./PowerShell.Tests/Includes/SampleDb.kdbx"),
                MasterKey = "testpassword".ConvertToSecureString(),
                KeyPath = Path.GetFullPath(@"./PowerShell.Tests/Includes/SampleDb.key")
            };
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
            Assert.NotEmpty(commandRuntimeMock.OutputPipeline);
        }
    }
}