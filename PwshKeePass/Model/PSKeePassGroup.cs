using System;
using System.Diagnostics.CodeAnalysis;
using KeePassLib;
using KeePassLib.Collections;

namespace PwshKeePass.Model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PSKeePassGroup
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string ParentGroup { get; set; }
        public string FullPath { get; set; }

        public DateTime CreationTime { get; set; }
        public bool Expires { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastModificationTime { get; set; }
        public DateTime LocationChanged { get; set; }

        public PwObjectList<PwGroup> Groups { get; set; }
        public ulong UsageCount { get; set; }
        public int EntryCount { get; set; }

        public PwIcon IconId { get; set; }
        public PwGroup KPGroup { get; set; }
    }
}