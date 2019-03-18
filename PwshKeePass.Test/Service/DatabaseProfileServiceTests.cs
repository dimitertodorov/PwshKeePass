
using PwshKeePass.Common.Extensions;
using Xunit;

namespace PwshKeePass.Test
{
    public class DatabaseProfileServiceTests :  IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        
        public DatabaseProfileServiceTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void TestNewKeePassDatabaseConfiguration()
        {
            var dbProfile = _fixture.DatabaseProfileService.NewKeePassDatabaseConfiguration("KeyAndMaster",
                _fixture.DatabaseProfileName, _fixture.DatabasePath, _fixture.KeyPath, _fixture.MasterKey.ConvertToSecureString(),
                false, false);
            dbProfile.Connect();
            Assert.True(dbProfile.PwDatabase.IsOpen);
        }
        
        [Fact]
        public void TestGetDatabaseProfile()
        {
            var dbProfile = _fixture.DatabaseProfileService.GetDatabaseProfile(_fixture.DatabaseProfileName);
            Assert.True(dbProfile.PwDatabase.IsOpen);
        }
    }
}