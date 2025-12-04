using Car_rental_management_system.Enum;
using Car_rental_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_rental_management_system.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public ReservationStatus Status { get; set; }
        public DateOnly PickupStartDate { get; set; }
        public TimeOnly PickupStartTime { get; set; }
        public DateOnly PickupEndDate { get; set; }
        public TimeOnly PickupEndTime { get; set; }

        [NotMapped]
        public DateTime PickupStartDateTime =>
        PickupStartDate.ToDateTime(PickupStartTime);

        [NotMapped]
        public DateTime PickupEndDateTime =>
            PickupEndDate.ToDateTime(PickupEndTime);
        public double Final_amount { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public string AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        public double CalculateFinalAmount()
        {
            if (Car == null)
                throw new Exception("CalculateFinalAmount() called too early: Car is NULL");
            var plan = Car?.Plan ?? throw new Exception("Car has no pricing plan.");

            double units = plan.Plan_type switch
            {
                plan_type.Hourly => (PickupEndDateTime - PickupStartDateTime).TotalHours,
                plan_type.Daily => (PickupEndDateTime - PickupStartDateTime).TotalDays,
                _ => throw new Exception("Unsupported pricing type.")
            };

            return units * plan.PricePerUnit;
        }



    }
}
