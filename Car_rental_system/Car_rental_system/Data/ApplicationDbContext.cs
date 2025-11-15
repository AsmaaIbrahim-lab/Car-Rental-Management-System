using Car_rental_system.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Car_rental_system.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Users> users { get; set; }
        public DbSet <Customer> customers { get; set; }
        public DbSet<CarOwner> carOwners { get; set; }
        public DbSet<Admin> Admins { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Users>(L =>
            {
                L.HasIndex(N => N.LicenseNumber).IsUnique();

                L.Property(u => u.Age)
                .HasComputedColumnSql("DATEDIFF(year, DateOfBirth, GETDATE())");

            });
            builder.Entity<Users>()
                .HasOne(u => u.customer)
                .WithOne(c => c.Users)
                .HasForeignKey<Customer>(c => c.UserId);

            builder.Entity<Users>()
               .HasOne(u => u.carOwner)
               .WithOne(c => c.Users)
               .HasForeignKey<CarOwner>(c => c.UserId);

            builder.Entity<Users>()
              .HasOne(u => u.Admin)
              .WithOne(c => c.Users)
              .HasForeignKey<Admin>(c => c.UserId);


        }
       

    }
}
