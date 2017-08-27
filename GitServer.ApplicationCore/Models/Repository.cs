using System;
using System.Collections.Generic;

namespace GitServer.ApplicationCore.Models
{
    public partial class Repository
    {
        public Repository()
        {
            this.TeamRepositoryRoles = new List<TeamRepositoryRole>();
        }

        public long ID { get; set; }
        public long UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsPrivate { get; set; }
        public bool AllowAnonymousRead { get; set; }
        public bool AllowAnonymousWrite { get; set; }
        public DateTime UpdateTime { get; set; }
        public virtual ICollection<TeamRepositoryRole> TeamRepositoryRoles { get; set; }
    }
}
