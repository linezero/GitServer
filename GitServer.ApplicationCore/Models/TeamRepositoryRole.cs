using System;
using System.Collections.Generic;

namespace GitServer.ApplicationCore.Models
{
    public class TeamRepositoryRole
    {
        public long TeamID { get; set; }
        public long RepositoryID { get; set; }
        public bool AllowRead { get; set; }
        public bool AllowWrite { get; set; }
        public virtual Repository Repository { get; set; }
        public virtual Team Team { get; set; }
    }
}
