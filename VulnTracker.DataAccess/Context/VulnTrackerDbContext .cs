using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulnTracker.Domain.Entities;

namespace VulnTracker.DataAccess.Context
{
    public class VulnTrackerDbContext : DbContext
    {
        public DbSet<Vulnerability> Vulnerabilities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

        public VulnTrackerDbContext(DbContextOptions<VulnTrackerDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vulnerability>().HasKey(v => v.Id);
            modelBuilder.Entity<Vulnerability>()
                        .HasOne(v => v.Project)
                        .WithMany(p => p.Vulnerabilities)
                        .HasForeignKey(v => v.ProjectId);

            modelBuilder.Entity<Project>().HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }

}
