using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Security;
using KeePassLib;

namespace PwshKeePass.Model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PSKeePassEntry
    {
        public string UserName { get; set; }
        public string Uuid { get; set; }
        public string ParentGroup { get; set; }
        public string FullPath { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }

        public List<string> Tags { get; set; }

        public DateTime CreationTime { get; set; }
        public bool Expires { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastModificationTime { get; set; }
        public DateTime LocationChanged { get; set; }

        public ulong UsageCount { get; set; }

        public PwIcon IconId { get; set; }
        public PwEntry KPEntry { get; set; }

        public PSCredential Credential { get; set; }
        public SecureString Password { get; set; }
    }
}