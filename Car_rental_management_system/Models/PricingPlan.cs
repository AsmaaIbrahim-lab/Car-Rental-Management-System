using Car_rental_management_system.Enum;
using System.ComponentModel.DataAnnotations;

namespace Car_rental_management_system.Models
{
    public class PricingPlan
    {
        [Key]
        public int PlanId { get; set; }
        public plan_type Plan_type { get; set; }
        public double PricePerUnit { get; set; }
        public string ?description { get; set; }
        public virtual List<Car> Cars { get; set; }


    }
}
