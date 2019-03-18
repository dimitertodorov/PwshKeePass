using System;
using System.IO;
using System.Management.Automation;
using System.Security;
using KeePassLib;
using Moq;
using PwshKeePass.Common;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Profile;
using PwshKeePass.Service;
using PwshKeePass.Test.Mocks;

namespace PwshKeePass.Test
{
    public class PwshKeePassTestFixture : IDisposable
    {
        public readonly KeePassProfile KeePassProfile;
        public readonly DatabaseProfileService DatabaseProfileService;
        public readonly EntryService EntryService;
        public readonly GroupService GroupService;
        public readonly PasswordService PasswordService;

        public string DatabasePath;

        public string MasterKey = "testpassword";
        public string OriginalDatabasePath = @"./SampleFiles/SampleDb.kdbx";
        public string KeyPath = @"./SampleFiles/SampleDb.key";
        public string DatabaseProfileName = "testprofile";

        public readonly KeePassCmdlet MockCmdlet;

        public PwshKeePassTestFixture()
        {
            KeePassProfile = new KeePassProfile();
            KeePassProfile.TestProfile = KeePassProfile;
            var commandRuntimeMock = new MockCommandRuntime();
            MockCmdlet = new TestKeePassCmdlet()
            {
                CommandRuntime = commandRuntimeMock
            };
            DatabasePath =
                OriginalDatabasePath.Replace(Path.GetFileName(OriginalDatabasePath), Path.GetRandomFileName());
            File.Copy(OriginalDatabasePath, DatabasePath);
            DatabaseProfileService = new DatabaseProfileService(KeePassProfile, MockCmdlet);
            KeePassProfile.TestProfile.ActiveProfiles[DatabaseProfileName] = DatabaseProfileService.NewKeePassDatabaseConfiguration(
                "KeyAndMaster",
                DatabaseProfileName, DatabasePath, KeyPath, MasterKey.ConvertToSecureString(),
                false, false);
            KeePassProfile.TestProfile.ActiveProfiles[DatabaseProfileName].Connect();
            EntryService = new EntryService(KeePassProfile, MockCmdlet,
                KeePassProfile.TestProfile.ActiveProfiles[DatabaseProfileName].PwDatabase);
            GroupService = new GroupService(KeePassProfile, MockCmdlet,
                KeePassProfile.TestProfile.ActiveProfiles[DatabaseProfileName].PwDatabase);
            PasswordService = new PasswordService(KeePassProfile.TestProfile, MockCmdlet);
        }

        public PwDatabase GetConnection()
        {
            return KeePassProfile.TestProfile.ActiveProfiles[DatabaseProfileName].PwDatabase;
        }

        public void Dispose()
        {
            File.Delete(Path.GetFullPath(DatabasePath));
        }
    }
}