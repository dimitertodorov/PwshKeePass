using System;
using PwshKeePass.Service;
using Xunit;

namespace PwshKeePass.Test
{
    [Collection("CmdletTests")]
    public class PasswordServiceTests :  IClassFixture<PwshKeePassTestFixture>
    {
        private readonly PwshKeePassTestFixture _fixture;
        
        public PasswordServiceTests(PwshKeePassTestFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void TestNewKeePassPasswordProfile()
        {
            var pwProfile = _fixture.PasswordService.NewKeePassPasswordProfile(
                upperCase: true,
                lowerCase: true,
                digits: true,
                specialCharacters: true,
                minus: true,
                underScore: true,
                space: false,
                brackets: false,
                excludeLookAlike: true,
                noRepeatingCharacters: false,
                excludeCharacters: null,
                length: 16,
                saveAs: null);
            Assert.NotNull(pwProfile);
        }
        
    }
}