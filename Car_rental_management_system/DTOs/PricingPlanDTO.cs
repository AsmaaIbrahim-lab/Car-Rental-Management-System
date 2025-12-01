using Car_rental_management_system.Enum;

namespace Car_rental_management_system.DTOs
{
    public class PricingPlanDTO
    {
        public plan_type Plan_type { get; set; }
        public double PricePerUnit { get; set; }
    }
}
