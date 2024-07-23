using AlleyCatBarbers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlleyCatBarbers.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceId);

            // Configure the relationship between Booking and IdentityUser
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<AlleyCatBarbers.Models.Email> Email { get; set; } = default!;
    }
}
