using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Security;
using KeePassLib;
using PwshKeePass.Common;
using PwshKeePass.Profile;

namespace PwshKeePass.Service
{
    /// <summary>
    /// Service class handling 
    /// </summary>
    public class DatabaseProfileService : KeePassService
    {
        public const string MasterParameterSet = "Master";
        public const string KeyAndMasterParameterSet = "KeyAndMaster";

        public DatabaseProfileService(KeePassProfile keePassProfile, KeePassCmdlet keePassCmdlet) : base(keePassProfile,
            keePassCmdlet)
        {
        }

        public DatabaseProfile GetDatabaseProfile(string databaseProfileName, bool connect = false)
        {
            if (KeePassProfile.ActiveProfiles.ContainsKey(databaseProfileName))
            {
                if (KeePassProfile.ActiveProfiles[databaseProfileName].PwDatabase == null)
                {
                    if (connect)
                    {
                        KeePassProfile.ActiveProfiles[databaseProfileName].Connect();
                    }
                    else
                    {
                        KeePassProfile.ActiveProfiles[databaseProfileName].PwDatabase = new PwDatabase();
                    }

                    return KeePassProfile.ActiveProfiles[databaseProfileName];
                }

                if (!KeePassProfile.ActiveProfiles[databaseProfileName].PwDatabase.IsOpen)
                {
                    try
                    {
                        if (connect)
                            KeePassProfile.ActiveProfiles[databaseProfileName].Connect();
                    }
                    catch (Exception)
                    {
                        throw new PSInvalidOperationException($"Database Profile ({databaseProfileName}) not open.");
                    }
                }

                return KeePassProfile.ActiveProfiles[databaseProfileName];
            }

            return null;
        }

        public DatabaseProfile GetDatabaseProfile(string parameterSetName, string databasePath,
            string keyPath, SecureString masterKey)
        {
            DatabaseProfile dbProfile = new DatabaseProfile();

            var masterParameterSets = new List<string> {MasterParameterSet, KeyAndMasterParameterSet};
            if (masterParameterSets.Contains(parameterSetName, StringComparer.OrdinalIgnoreCase))
            {
                if (masterKey == null)
                {
                    masterKey = GetPassword("Enter Master Key: ");
                }

                dbProfile.UseMasterKey = true;
                dbProfile.MasterKey = masterKey;
            }

            dbProfile.Name = "Transient";
            if (!String.IsNullOrEmpty(keyPath))
                dbProfile.KeyPath = Path.GetFullPath(keyPath);

            dbProfile.DatabasePath = Path.GetFullPath(databasePath);

            return dbProfile;
        }

        public DatabaseProfile NewKeePassDatabaseConfiguration(string parameterSetName, string databaseProfileName,
            string databasePath,
            string keyPath, SecureString masterKey, bool setAsDefault,
            bool saveToDisk, bool force = false)
        {
            if (KeePassProfile.ActiveProfiles.ContainsKey(databaseProfileName))
            {
                if (!force)
                {
                    var err = new ErrorRecord(
                        new ArgumentException(
                            $"KeePassDatabaseConfiguration with name {databaseProfileName} already exists. Specify -Force to OverWrite"),
                        string.Empty, ErrorCategory.NotSpecified, Cmdlet);
                    Cmdlet.WriteError(err);
                }
            }

            var dbProfile = GetDatabaseProfile(parameterSetName, databasePath, keyPath, masterKey);
            dbProfile.Name = databaseProfileName;
            dbProfile.AuthenticationType = parameterSetName;

            if (setAsDefault || string.IsNullOrEmpty(KeePassProfile.DefaultProfileName))
            {
                KeePassProfile.DefaultProfileName = databaseProfileName;
            }

            KeePassProfile.ActiveProfiles[databaseProfileName] = dbProfile;


            if (saveToDisk)
                KeePassProfile.SaveToDisk();

            KeePassProfile.DefaultProfile = KeePassProfile;
            return dbProfile;
        }

        public void RemoveKeePassDatabaseConfiguration(string databaseProfileName)
        {
            if (KeePassProfile.ActiveProfiles.ContainsKey(databaseProfileName))
            {
                KeePassProfile.ActiveProfiles.Remove(databaseProfileName);
                KeePassProfile.SaveToDisk();
            }

            KeePassProfile = KeePassProfile.SyncFromDisk();
        }
    }
}