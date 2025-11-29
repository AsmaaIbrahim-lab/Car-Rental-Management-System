using Car_rental_management_system.Enum;
using Car_rental_system.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_rental_management_system.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public CarStatus Status { get; set; }


        [Required(ErrorMessage = "Model is required.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Model must be between 2 and 50 characters.")]
        public string Model { get; set; }


        [Required(ErrorMessage = "Number is required.")]
        [StringLength(15, MinimumLength = 5,
            ErrorMessage = "Number must be between 5 and 15 characters.")]
        [RegularExpression(@"^[\u0621-\u064A]{3}\s[0-9]{1,4}$",
    ErrorMessage = "Plate must be 3 Arabic letters, space, then 1–4 digits.")]
        public string Number { get; set; }



        [Required(ErrorMessage = "Color is required.")]
        [RegularExpression(@"^[A-Za-z]{3,20}$",
            ErrorMessage = "Color must contain only letters and be 3–20 characters long.")]
        public string Color { get; set; }


        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location must not exceed 100 characters.")]
        public string Location { get; set; }

        public int OwnerId { get; set; }      
        public  virtual CarOwner Owner { get; set; }
        public int? AdminId { get; set; } 
        public string Type { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual List<Car_CarImage> CarImages { get; set; }

        public int? PlanId { get; set; }

        public virtual PricingPlan Plan { get; set; }





    }
}
