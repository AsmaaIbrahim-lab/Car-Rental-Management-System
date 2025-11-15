using System.ComponentModel.DataAnnotations;

namespace Car_rental_system.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public string UserId { get; set; }
        public Users Users { get; set; }
    }
}
