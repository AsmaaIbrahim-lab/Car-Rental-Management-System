using Car_rental_system.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Car_rental_system.Models
{
    [Index(nameof(LicenseNumber), IsUnique = true)]

    public class Users : IdentityUser
    {
        [Key]
        public override string Id { get; set; }

     
        public string LicenseNumber { get; set; }

        public DateOnly ExpiryDate { get; set; }

        public string LicenseType { get; set; }

        public Role Role { get; set; }

      
        public override string Email { get; set; }

       
        public override string UserName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public int Age => DateOnly.FromDateTime(DateTime.Now).Year - DateOfBirth.Year;

        [Required]
        public Gender gender { get; set; }

      
        public override string PhoneNumber { get; set; }

        public City City { get; set; }

        public string Country { get; set; }

        public virtual Customer customer { get; set; }
        public virtual CarOwner carOwner { get; set; }
        public virtual Admin Admin { get; set; }
    }
}

