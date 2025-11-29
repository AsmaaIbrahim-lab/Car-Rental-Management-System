using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Car_rental_system.Enum;

namespace Car_rental_system.Models
{
    public class Users : IdentityUser
    {
        [Key]
        public override string Id { get; set; } 

        [Required]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}[0-9]{6}$",
            ErrorMessage = "License number must be 2 uppercase letters followed by 6 digits")]
        public string LicenseNumber { get; set; }

        public Role Role { get; set; }

        [EmailAddress]
        [Required]
        [MaxLength(80)]
        public override string Email { get; set; } 

        

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,20}$",
            ErrorMessage = "Username must be 3-20 characters with letters and digits only")]
        public override string UserName { get; set; }
        public string? ImageUrl { get; set; }
        public string? LisenceImageUrl { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        
        public DateOnly DateOfBirth { get; set; }

        public int Age => DateOnly.FromDateTime(DateTime.Now).Year - DateOfBirth.Year;

        [Required]
        public Gender gender { get; set; }

        [Required]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",
            ErrorMessage = "Phone number must be a valid Egyptian number")]
        public override string PhoneNumber { get; set; } 

        [Required]
        public City City { get; set; }

        [Required]
        public string Country { get; set; } = "Egypt";

        public virtual Customer customer { get; set; }
        public virtual  CarOwner carOwner { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
