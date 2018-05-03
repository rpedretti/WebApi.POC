using Microsoft.EntityFrameworkCore;
using WebApi.POC.Domain;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local
{
    /// <summary>
    /// Local database repository
    /// </summary>
    public class PocDbContext : DbContext
    {
        /// <summary>
        /// The users set
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// The services set
        /// </summary>
        public DbSet<ServiceDemand> ServiceDemands { get; set; }

        /// <summary>
        /// The cryptographic keys set
        /// </summary>
        public DbSet<CryptoKey> CryptoKeys { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Db creations options</param>
        public PocDbContext(DbContextOptions<PocDbContext> options) : base(options) { }

        /// <summary>
        /// Configures the relations at the database
        /// </summary>
        /// <param name="modelBuilder"></param>
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
