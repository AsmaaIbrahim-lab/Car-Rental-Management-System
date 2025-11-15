using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Car_rental_system.Helpers.Validation;
using Car_rental_system.Enum;

namespace Car_rental_system.Models
{
    public class Users:IdentityUser
    {
        [Key]
        public string Id {  get; set; }
        [Required]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}[0-9]{6}$",
        ErrorMessage = "رقم الرخصة يجب أن يكون حرفين كبيرين متبوعين بـ 6 أرقام")]
        public string LicenseNumber { get; set; }

        public Role Role { get; set; }
        [EmailAddress]
        [Required]
        [MaxLength(80)]

        public string Email { get; set; }

        [MaxLength(40)]
        public string ImageUrl { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,20}$",
        ErrorMessage = "اسم المستخدم يجب أن يكون من 3 إلى 20 حرفًا، ويحتوي على حروف وأرقام فقط")]
        public string UserName { get; set; }
        [MaxLength(40)]
        public string LisenceImageUrl { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "الباسورد يجب أن يكون بين 8 و 20 حرف.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,20}$",
         ErrorMessage = "الباسورد يجب أن يحتوي على حرف كبير، حرف صغير، رقم، رمز خاص وطوله بين 8 و20 حرف.")]
        public string password { get; set; }
        [Required(ErrorMessage = "تاريخ الميلاد مطلوب.")]
        [DataType(DataType.Date)]
        [DOFValidation]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        [Required]

        public Gender gender { get; set; }
        [Required]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",
    ErrorMessage = "رقم الموبايل يجب أن يكون رقم مصري صالح من 11 رقم")]
        public string PhoneNumber {  get; set; }
        [Required]
        public City City { get; set; }

        [Required]
        public string Country { get; set; } = "Egypt";
        public Customer customer { get; set; }
        public CarOwner carOwner { get; set; }
        public Admin Admin { get; set; }







    }
}
