using Car_rental_management_system.Models;
using Car_rental_system.Enum;
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
        public DbSet<Car> Cars { get; set; }
        public DbSet<Car_CarImage> Car_Image { get; set; }
        public DbSet<PricingPlan> Plans { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Users>(L => L.Property(u => u.LicenseNumber)
                        .IsRequired());
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

            builder.Entity<Car>()
                .HasOne(c => c.Owner)
                .WithMany(co => co.Cars)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Car>()
                .HasOne(c => c.Admin)
                .WithMany(a => a.Cars)
                .HasForeignKey(c => c.AdminId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.SetNull);

            

            builder.Entity<Car_CarImage>()
           .HasKey(cI => new { cI.CarId, cI.ImagePath });

            builder.Entity<Car_CarImage>()
            .HasOne(cI => cI.Car)
            .WithMany(c => c.CarImages)
           .HasForeignKey(cI => cI.CarId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PricingPlan>()
             .Property(p => p.Plan_type)
               .HasConversion<string>();

            builder.Entity<Car>()
             .Property(c => c.Status)
               .HasConversion<string>();
            builder.Entity<Car_CarImage>()
           .Property(c => c.ImageType)
             .HasConversion<string>();

            builder.Entity<Car_CarImage>()
          .Property(c => c.ImageType)
            .HasConversion<string>();

            builder.Entity<PricingPlan>()
      .Property(p => p.Plan_type)
        .HasConversion<string>();
            builder.Entity<Users>()
              .Property(p => p.City)
              .HasConversion<string>();

            builder.Entity<Users>()
            .Property(p => p.gender)
            .HasConversion<string>();

            builder.Entity<Users>()
          .Property(p => p.Role)
          .HasConversion<string>();

            builder.Entity<Car>()
                   .HasOne(c => c.Plan)
                   .WithMany(co => co.Cars)
                   .HasForeignKey(c => c.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
       

    }
}
