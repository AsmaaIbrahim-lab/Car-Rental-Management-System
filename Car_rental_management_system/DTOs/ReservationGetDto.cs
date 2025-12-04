using Car_rental_management_system.Enum;
using Car_rental_management_system.Models;
using Car_rental_system.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_rental_management_system.DTOs
{
    public class ReservationGetDto
    {
        public int ReservationId { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerLicenseNumber { get; set; }

        public int CarId { get; set; }
        public string CarModel { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
        public double CarPrice { get; set; } 
        public string CarLocationPickup { get; set; }
        public string CarLocationDropoff { get; set; }

        public DateTime PickupDateTime { get; set; }
        public DateTime DropoffDateTime { get; set; }

        public City CustomerCity { get; set; }
    }

}
