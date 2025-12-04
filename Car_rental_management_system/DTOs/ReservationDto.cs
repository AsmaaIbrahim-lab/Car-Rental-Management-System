using Car_rental_management_system.Enum;
using Car_rental_management_system.Models;
using Car_rental_system.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_rental_management_system.DTOs
{
    public class ReservationDto
    {
        public DateOnly PickupStartDate { get; set; }
        
        public DateOnly PickupEndDate { get; set; }
        public TimeOnly pickupStartTime { get; set; }
        public TimeOnly pickupEndTime { get; set; }
        public int CarId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
    }
}
