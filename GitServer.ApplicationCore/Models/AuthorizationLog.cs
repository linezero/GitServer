using System;
using System.Collections.Generic;

namespace GitServer.ApplicationCore.Models
{
    public partial class AuthorizationLog
    {
        public Guid ID { get; set; }
        public long UserID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime Expires { get; set; }
        public string IssueIp { get; set; }
        public string LastIp { get; set; }
        public bool IsValid { get; set; }
        public virtual User User { get; set; }
    }
}
