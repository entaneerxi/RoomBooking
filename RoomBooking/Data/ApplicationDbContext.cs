using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Models;

namespace RoomBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<MonthlyRental> MonthlyRentals { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<HomepageSlide> HomepageSlides { get; set; }
        public DbSet<BookingAddon> BookingAddons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for financial fields
            modelBuilder.Entity<Room>()
                .Property(r => r.DailyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Room>()
                .Property(r => r.MonthlyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.FinalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.WaterBill)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.ElectricityBill)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.PreviousWaterReading)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.CurrentWaterReading)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.PreviousElectricityReading)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.CurrentElectricityReading)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.WaterUnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.ElectricityUnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BookingAddon>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            // Configure relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MonthlyRental>()
                .HasOne(m => m.Booking)
                .WithOne(b => b.MonthlyRental)
                .HasForeignKey<MonthlyRental>(m => m.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            modelBuilder.Entity<ContactInfo>().HasData(
                new ContactInfo
                {
                    Id = 1,
                    PropertyName = "Room Booking System",
                    Address = "123 Main Street, City, Country",
                    Phone = "+1234567890",
                    Email = "info@roombooking.com",
                    Description = "Welcome to our room booking system",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
