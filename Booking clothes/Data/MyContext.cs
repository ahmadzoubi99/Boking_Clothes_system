using Booking_clothes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Booking_clothes.Data
{
    public class MyContext : IdentityDbContext<IdentityUser>
    {

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactUs> ContactUsMessages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationDetail> ReservationDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Products> Products { get; set; } // Added Clothes DbSet
        public DbSet<ProductSize> ProductSize { get; set; } // Added Clothes DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure foreign key relationships with Restrict behavior
        
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.ReservationDetails)
                .WithOne(rd => rd.Reservation)
                .HasForeignKey(rd => rd.ReservationId)
                .OnDelete(DeleteBehavior.NoAction);

         
        }

        public DbSet<Booking_clothes.Models.Banck>? Banck { get; set; }

        public DbSet<Booking_clothes.Models.Coupon>? Coupon { get; set; }

        public DbSet<Booking_clothes.Models.Address>? Address { get; set; }
    }
}
