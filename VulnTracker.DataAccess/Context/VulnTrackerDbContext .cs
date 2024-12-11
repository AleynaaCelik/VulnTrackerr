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

        public VulnTrackerDbContext(DbContextOptions<VulnTrackerDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vulnerability>().HasKey(v => v.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
