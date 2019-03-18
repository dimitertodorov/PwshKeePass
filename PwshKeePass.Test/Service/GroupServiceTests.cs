using System;
using PwshKeePass.Service;
using Xunit;

namespace PwshKeePass.Test
{
    [Collection("CmdletTests")]
    public class GroupServiceTests :  IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        
        public GroupServiceTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void TestGetGroups()
        {
            var entries = _fixture.GroupService.GetGroups();
            Assert.NotEmpty(entries);
        }
        
        [Fact]
        public void TestGetGroup()
        {
            var entry = _fixture.GroupService.GetGroup("SampleDb/12345/anotherGroup");
            Assert.NotNull(entry);
        }

        [Fact]
        public void TestGetRecycleBin()
        {
            var entry = _fixture.GroupService.GetRecycleBin();
            Assert.Equal("Recycle Bin", entry.Name);
        }
    }
}