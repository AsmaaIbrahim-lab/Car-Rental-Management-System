using Car_rental_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace Car_rental_system.Models
{
    public class Admin
    {
        [Key]
        public string AdminId { get; set; }

        public virtual Users User { get; set; }
        public virtual List<Car> Cars { get; set; }
        public virtual List<Reservation> Reservations { get; set; }

    }
}
