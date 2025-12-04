using Car_rental_management_system.Enum;
using System.ComponentModel.DataAnnotations;

namespace Car_rental_management_system.DTOs
{


    public class CarCreateDto
    {
        [Required]
        public string Model { get; set; }

        [Required]
        public string Number { get; set; }

        public CarStatus Status { get; set; } 

        public string Color { get; set; }

        public string Location { get; set; }
        public string Type { get; set; }

        public int PlanId { get; set; }
        public string Describtion { get; set; }


        public List<IFormFile> Images { get; set; }

        public ImageType ImageType { get; set; }
    }

    
}
