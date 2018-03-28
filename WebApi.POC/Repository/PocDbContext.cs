using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.POC.Domain;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository
{
    public class PocDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ServiceDemand> ServiceDemands { get; set; }
        public DbSet<CryptoKey> CryptoKeys { get; set; }

        public PocDbContext(DbContextOptions<PocDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<ServiceDemand>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Status>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<CryptoKey>()
                .HasOne(c => c.Kind)
                .WithMany();

            modelBuilder.Entity<CryptoKey>()
                .HasKey(s => new { s.Id, s.KindId });

            modelBuilder.Entity<CryptoKey>()
                .HasOne(c => c.Kind)
                .WithMany();

            modelBuilder.Entity<KeyKind>()
                .HasKey(k => k.Id);
                
        }
    }
}
