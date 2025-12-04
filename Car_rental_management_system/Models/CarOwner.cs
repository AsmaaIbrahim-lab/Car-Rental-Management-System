using Car_rental_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace Car_rental_system.Models
{
    public class CarOwner
    {
        [Key]
        public string CarOwnerId { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

    }
}
