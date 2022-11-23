using AireBugTracker.Shared;
using Microsoft.EntityFrameworkCore;

namespace AireBugTracker.Data
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() { }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Bug> Bugs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {

                entity.ToTable("Users");
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            });
            modelBuilder.Entity<Bug>(entity =>
            {
                entity.ToTable("Bugs");
                entity.Property(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Description).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.OpenedDate);
                entity.Property(e => e.ClosedDate);

            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
