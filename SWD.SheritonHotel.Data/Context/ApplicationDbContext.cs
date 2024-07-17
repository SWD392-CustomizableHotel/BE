using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Amenity> Amenity { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<BookingService> BookingService { get; set; }
        public DbSet<BookingAmenity> BookingAmenity { get; set; }
        public DbSet<IdentityCard> IdentityCard { get; set; }
        public DbSet<AssignedService> AssignedServices { get; set; }
        public DbSet<ServiceStaff> ServiceStaff { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure primary keys
            builder.Entity<Payment>().HasKey(p => p.Id);
            builder.Entity<Service>().HasKey(s => s.Id);
            builder.Entity<Amenity>().HasKey(a => a.Id);
            builder.Entity<Booking>().HasKey(b => b.Id);
            builder.Entity<Room>().HasKey(rm => rm.Id);
            builder.Entity<Hotel>().HasKey(h => h.Id);
            builder.Entity<IdentityCard>().HasKey(ic => ic.Id);

            builder.Entity<BookingService>().HasKey(bs => new { bs.BookingId, bs.ServiceId });
            builder.Entity<BookingAmenity>().HasKey(ba => new { ba.BookingId, ba.AmenityId });
            builder.Entity<ServiceStaff>().HasKey(ss => new { ss.ServiceId, ss.UserId }); // Composite key for ServiceStaff
            builder.Entity<AssignedService>().HasKey(be => new { be.AssignedServiceId });

            // Configure relationships
            builder
                .Entity<AssignedService>()
                .HasOne(be => be.Service)
                .WithMany(u => u.AssignedServices)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .Entity<AssignedService>()
                .HasOne(be => be.User)
                .WithMany(u => u.AssignedServices)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ApplicationUser>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            builder
                .Entity<Booking>()
                .HasMany(b => b.Payments)
                .WithOne(p => p.Booking)
                .HasForeignKey(p => p.BookingId);

            builder
                .Entity<Booking>()
                .HasMany(b => b.BookingServices)
                .WithOne(bs => bs.Booking)
                .HasForeignKey(bs => bs.BookingId);

            builder
                .Entity<Booking>()
                .HasMany(b => b.BookingAmenities)
                .WithOne(ba => ba.Booking)
                .HasForeignKey(ba => ba.BookingId);

            builder
                .Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId);

            builder
                .Entity<Room>()
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Room)
                .HasForeignKey(b => b.RoomId);

            builder
                .Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId);

            builder.Entity<Room>().Property(r => r.Price).HasPrecision(18, 2);

            builder
                .Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId);

            builder
                .Entity<Hotel>()
                .HasMany(h => h.Amenities)
                .WithOne(a => a.Hotel)
                .HasForeignKey(a => a.HotelId);

            builder.Entity<Amenity>().Property(a => a.Price).HasPrecision(18, 2);

            builder
                .Entity<Service>()
                .HasMany(s => s.BookingServices)
                .WithOne(bs => bs.Service)
                .HasForeignKey(bs => bs.ServiceId);

            builder
                .Entity<Service>()
                .HasOne(s => s.Hotel)
                .WithMany(h => h.Services)
                .HasForeignKey(s => s.HotelId);

            builder.Entity<Service>().Property(s => s.Price).HasPrecision(18, 2);

            builder.Entity<Service>()
                .HasMany(s => s.AssignedStaff)
                .WithMany(u => u.AssignedServiceS)
                .UsingEntity<ServiceStaff>(
                    j => j.HasOne(ss => ss.ApplicationUser).WithMany().HasForeignKey(ss => ss.UserId),
                    j => j.HasOne(ss => ss.Service).WithMany().HasForeignKey(ss => ss.ServiceId)
                );

            builder
                .Entity<BookingService>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingServices)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<BookingService>()
                .HasOne(bs => bs.Service)
                .WithMany(s => s.BookingServices)
                .HasForeignKey(bs => bs.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<BookingAmenity>()
                .HasOne(ba => ba.Booking)
                .WithMany(b => b.BookingAmenities)
                .HasForeignKey(ba => ba.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<BookingAmenity>()
                .HasOne(ba => ba.Amenity)
                .WithMany(a => a.BookingAmenities)
                .HasForeignKey(ba => ba.AmenityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.IdentityCard)
                .WithOne(ic => ic.Payment)
                .HasForeignKey<IdentityCard>(ic => ic.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId);

            builder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
            builder.Entity<Payment>().Property(p => p.PaymentMethod).IsRequired(false);

            // Enum configuration
            builder
                .Entity<Service>()
                .Property(a => a.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (ServiceStatus)Enum.Parse(typeof(ServiceStatus), v))
                .IsRequired();

            // Enum configuration
            builder
                .Entity<Amenity>()
                .Property(a => a.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (AmenityStatus)Enum.Parse(typeof(AmenityStatus), v))
                .IsRequired();

            // Configure properties with max length
            builder.Entity<IdentityCard>()
                .Property(b => b.Nationality)
                .IsRequired()
                .HasMaxLength(50);

            builder.Entity<IdentityCard>()
                .Property(b => b.Gender)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}