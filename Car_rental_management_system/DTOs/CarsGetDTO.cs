using Car_rental_management_system.Enum;

namespace Car_rental_management_system.DTOs
{
    public class CarsGetDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }


        public CarStatus Status { get; set; }

        public string Color { get; set; }

        public string Type { get; set; }
        public PricingPlanDTO PricingPlan { get; set; }


    }
}
