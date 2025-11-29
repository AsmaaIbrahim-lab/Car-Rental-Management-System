using Car_rental_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace Car_rental_system.Models
{
    public class Admin
    {
        [Key]
        public  int AdminId { get; set; }
        public string UserId { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

    }
}
