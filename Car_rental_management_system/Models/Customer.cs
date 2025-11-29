using System.ComponentModel.DataAnnotations;

namespace Car_rental_system.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string UserId { get; set; }
        public virtual Users Users { get; set; }
    }
}
