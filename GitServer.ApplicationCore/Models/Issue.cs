using System;
using System.Collections.Generic;
using System.Text;

namespace GitServer.ApplicationCore.Models
{
    public class Issue
    {
        public Guid ID { get; set; }
        public long UserID { get; set; }
        public long RepositoryID { get; set; }
        public int IssueID { get; set; }
        public int TypeID { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public byte Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
