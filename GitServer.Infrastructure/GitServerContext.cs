using GitServer.ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace GitServer.Infrastructure
{
    public partial class GitServerContext : DbContext
    {
        public GitServerContext(DbContextOptions<GitServerContext> options)
            : base(options)
        {
        }

        public DbSet<AuthorizationLog> AuthorizationLogs { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<SshKey> SshKeys { get; set; }
        public DbSet<TeamRepositoryRole> TeamRepositoryRoles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTeamRole> UserTeamRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TeamRepositoryRole>().HasKey(t => new { t.TeamID, t.RepositoryID });
            modelBuilder.Entity<UserTeamRole>().HasKey(t => new { t.UserID, t.TeamID });
        }
    }
}
