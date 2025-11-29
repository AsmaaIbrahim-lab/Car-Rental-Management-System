using Car_rental_management_system.Enum;

namespace Car_rental_management_system.Models
{
    public class Car_CarImage
    {
        public int CarId { get; set; }
        public string ImagePath { get; set; }
         public ImageType ImageType { get; set; }
        public virtual Car Car { get; set; }

    }
}
