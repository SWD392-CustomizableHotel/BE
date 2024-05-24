using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Core.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }
        public DbSet<BookingAmenity> BookingAmenities { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure primary keys
            builder.Entity<Payment>().HasKey(p => p.PaymentId);
            builder.Entity<Service>().HasKey(s => s.ServiceId);
            builder.Entity<Amenity>().HasKey(a => a.AmenityId);
            builder.Entity<Booking>().HasKey(b => b.BookingId);
            builder.Entity<Room>().HasKey(rm => rm.RoomId);
            builder.Entity<Hotel>().HasKey(h => h.HotelId);

            builder.Entity<BookingService>().HasKey(bs => new { bs.BookingId, bs.ServiceId });
            builder.Entity<BookingAmenity>().HasKey(ba => new { ba.BookingId, ba.AmenityId });

            //User
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            //Role
            builder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            //Booking

            builder.Entity<Booking>()
                .HasMany(b => b.Payments)
                .WithOne(p => p.Booking)
                .HasForeignKey(p => p.BookingId);

            builder.Entity<Booking>()
                 .HasMany(b => b.BookingServices)
                 .WithOne(b => b.Booking)
                 .HasForeignKey(bs => bs.BookingId);

            builder.Entity<Booking>()
                .HasMany(b => b.BookingAmenities)
                .WithOne(ba => ba.Booking)
                .HasForeignKey(ba => ba.BookingId);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            builder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId);

            //Room
            builder.Entity<Room>()
              .HasMany(r => r.Bookings)
              .WithOne(b => b.Room)
              .HasForeignKey(b => b.RoomId);

            builder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId);

            builder.Entity<Room>()
                .Property(r => r.Price)
                .HasPrecision(18, 2);

            //Hotel
            builder.Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId);

            builder.Entity<Hotel>()
                .HasMany(h => h.Amenities)
                .WithOne(a => a.Hotel)
                .HasForeignKey(a => a.HotelId);

            builder.Entity<Amenity>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);
            //Serive

            builder.Entity<Service>()
                .HasMany(s => s.BookingServices)
                .WithOne(bs => bs.Service)
                .HasForeignKey(bs => bs.ServiceId);

            builder.Entity<Service>()
                .HasOne(s => s.Hotel)
                .WithMany(h => h.Services)
                .HasForeignKey(s => s.HotelId);

            builder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            //Booking Service
            builder.Entity<BookingService>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingServices)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict); ;

            builder.Entity<BookingService>()
                .HasOne(bs => bs.Service)
                .WithMany(s => s.BookingServices)
                .HasForeignKey(bs => bs.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // BookingAmenity
            builder.Entity<BookingAmenity>()
                 .HasOne(ba => ba.Booking)
                 .WithMany(b => b.BookingAmenities)
                 .HasForeignKey(ba => ba.BookingId)
                 .OnDelete(DeleteBehavior.Restrict); // Change this line

            builder.Entity<BookingAmenity>()
                .HasOne(ba => ba.Amenity)
                .WithMany(a => a.BookingAmenities)
                .HasForeignKey(ba => ba.AmenityId)
                .OnDelete(DeleteBehavior.Restrict); // And this line

            // Payment
            builder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId);

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
        }
    }
}
