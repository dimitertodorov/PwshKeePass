using System;
using System.Management.Automation;
using System.Security;
using KeePassLib;
using KeePassLib.Security;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Model;

namespace PwshKeePass.Common
{
    public abstract class KeePassConnectedCmdlet : KeePassCmdlet
    {
        [Parameter(Mandatory = false)]
        public PwDatabase Connection { get; set; }

        [Parameter(Mandatory = false)]
        public string DatabaseProfileName { get; set; }

        protected PwEntry ResolveKeePassEntry(PSObject keePassEntry)
        {
            PwEntry keePassPwEntry;
            var kpe = keePassEntry.BaseObject;
            if (kpe is PSKeePassEntry)
            {
                var psKeePassEntry = (PSKeePassEntry) kpe;
                keePassPwEntry = psKeePassEntry.KPEntry;
            }
            else if (kpe is PwEntry)
            {
                keePassPwEntry = (PwEntry) kpe;
            }
            else
            {
                throw new PSArgumentOutOfRangeException("KeePassEntry", kpe.GetType().ToString(),
                    "KeePassEntry must be either PwEntry, or PSKeePassEntry.");
            }

            return keePassPwEntry;
        }

        protected PwGroup ResolveKeePassGroup(PSObject keePassGroup)
        {
            PwGroup pwGroup;
            var kpg = keePassGroup.BaseObject;
            switch (kpg)
            {
                case PSKeePassGroup psKeePassGroup:
                    pwGroup = psKeePassGroup.KPGroup;
                    break;
                case PwGroup group:
                    pwGroup = group;
                    break;
                default:
                    throw new PSArgumentOutOfRangeException("KeePassGroup", kpg.GetType().ToString(),
                        "KeePassEntry must be either PwEntry, or PSKeePassEntry.");
            }

            return pwGroup;
        }

        protected SecureString ResolveKeePassPassword(PSObject keePassPassword)
        {
            SecureString securePassword = null;
            if (keePassPassword != null)
            {
                var kps = keePassPassword.BaseObject;

                if (kps is SecureString)
                {
                    securePassword = (SecureString) kps;
                }
                else if (kps is ProtectedString)
                {
                    var protectedPassword = (ProtectedString) kps;
                    securePassword = protectedPassword.ReadString().ConvertToSecureString();
                }
                else if (kps is string)
                {
                    var stringPassword = (string) kps;
                    securePassword = stringPassword.ConvertToSecureString();
                }
                else
                {
                    throw new PSArgumentOutOfRangeException("keePassPassword", keePassPassword.GetType().ToString(),
                        "keePassPassword must be either SecureString, ProtectedString, or string.");
                }
            }

            return securePassword;
        }

        /// <summary>
        /// Cmdlet begin process. Write to logs, setup Http Tracing and initialize profile
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (Connection == null && !String.IsNullOrEmpty(DatabaseProfileName))
            {
                var dbProfile = DatabaseProfileService.GetDatabaseProfile(DatabaseProfileName, true);
                if (dbProfile == null)
                    throw new PSArgumentException(
                        $"InvalidKeePassConnection : -DatabaseProfileName ({DatabaseProfileName}) does not exist in configuration.",
                        DatabaseProfileName);
                Connection = dbProfile.PwDatabase;
            }
            else if (Connection == null)

            {
                throw new PSArgumentException(
                    "InvalidKeePassConnection : No KeePass -Connection passed, and no -DatabaseProfileName provided.",
                    DatabaseProfileName);
            }
        }
    }
}