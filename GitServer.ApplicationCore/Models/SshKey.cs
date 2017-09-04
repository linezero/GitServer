using System;
using System.Collections.Generic;

namespace GitServer.ApplicationCore.Models
{
    public class SshKey
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public string KeyType { get; set; }
        public string Fingerprint { get; set; }
        public string PublicKey { get; set; }
        public DateTime ImportData { get; set; }
        public DateTime LastUse { get; set; }
        public virtual User User { get; set; }
    }
}
