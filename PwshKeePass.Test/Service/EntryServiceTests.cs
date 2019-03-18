using System;
using System.IO;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Service;
using Xunit;

namespace PwshKeePass.Test
{
    [Collection("CmdletTests")]
    public class EntryServiceTests :  IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        
        public EntryServiceTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void TestGetEntry()
        {
            var entries = _fixture.EntryService.GetEntry(null, null, null, null);
            Assert.NotEmpty(entries);
        }
        
        [Fact]
        public void TestGetEntryByName()
        {
            var entries = _fixture.EntryService.GetEntry(null, "nested3", null, null);
            Assert.Single(entries);
        }
        
        [Fact]
        public void TestGetEntryByNameNonExistent()
        {
            var entries = _fixture.EntryService.GetEntry(null, "sadasdasdasd", null, null);
            Assert.Empty(entries);
        }
        
        [Fact]
        public void TestCreateEntry()
        {
            var password = "testpassword".ConvertToSecureString();
            var entryName = Path.GetRandomFileName();

            var entry = _fixture.EntryService.CreateEntry(entryName, entryName, password, "NOTES", "", null,
                "SampleDb/12345/anotherGroup");
            Assert.NotNull(entry);
        }
        
        [Fact]
        public void TestRemoveEntry()
        {
            var password = "testpassword".ConvertToSecureString();
            var entryName = Path.GetRandomFileName();
            var entry = _fixture.EntryService.CreateEntry(entryName, entryName, password, "NOTES", "", null,
                "SampleDb/12345/anotherGroup");
            _fixture.EntryService.RemoveEntry(entry.KPEntry,true);
            var entries = _fixture.EntryService.GetEntry(null, entryName, null, null);
            Assert.Empty(entries);
        }
        
        [Fact]
        public void TestRemoveEntryRecycle()
        {
            var password = "testpassword".ConvertToSecureString();
            var entryName = Path.GetRandomFileName();
            var entry = _fixture.EntryService.CreateEntry(entryName, entryName, password, "NOTES", "", null,
                "SampleDb/12345/anotherGroup");
            _fixture.EntryService.RemoveEntry(entry.KPEntry,false);
            var entries = _fixture.EntryService.GetEntry(null, entryName, null, null);
            Assert.NotEmpty(entries);
        }
    }
}