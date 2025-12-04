using Car_rental_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_rental_system.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; }   


        public virtual Users User { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
