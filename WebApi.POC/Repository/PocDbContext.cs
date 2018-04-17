using Microsoft.EntityFrameworkCore;
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
                .ToTable("user")
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasOne(c => c.Role)
                .WithMany();

            modelBuilder.Entity<ServiceDemand>()
                .ToTable("demands")
                .HasKey(s => s.Id);

            modelBuilder.Entity<Role>()
                .ToTable("role")
                .HasKey(r => r.Id);

            modelBuilder.Entity<Status>()
                .ToTable("status")
                .HasKey(s => s.Id);

            modelBuilder.Entity<CryptoKey>()
                .ToTable("crypto_keys")
                .HasOne(c => c.Kind)
                .WithMany();

            modelBuilder.Entity<CryptoKey>()
                .HasKey(s => new { s.Id, s.KindId });

            modelBuilder.Entity<KeyKind>()
                .ToTable("key_kind")
                .HasKey(k => k.Id);

        }
    }
}
