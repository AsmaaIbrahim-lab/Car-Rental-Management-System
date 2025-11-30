using Car_rental_system.Enum;
using System.ComponentModel.DataAnnotations;

namespace Car_rental_management_system.ViewModel
{
    public class RegisterationModel
    {
       
        public string UserName { get; set; }

    
        public string Email { get; set; }


        public string Password { get; set; } 

        
        public DateOnly DateOfBirth { get; set; }

        public Gender gender { get; set; }
        public string LicenseNumber { get; set; }

        public DateOnly ExpiryDate { get; set; }

        public string LicenseType { get; set; }


        public string PhoneNumber { get; set; }

        public City City { get; set; }
        public string Country { get; set; } = "Egypt";

       

        public Role Role { get; set; }
    }
}
