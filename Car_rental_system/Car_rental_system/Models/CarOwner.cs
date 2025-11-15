using System.ComponentModel.DataAnnotations;

namespace Car_rental_system.Models
{
    public class CarOwner
    {

        [Key]
        public int CarOwnerId { get; set; }
        public string UserId { get; set; }
        public Users Users { get; set; }
    }
}
