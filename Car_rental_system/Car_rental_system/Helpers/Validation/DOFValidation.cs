using System.ComponentModel.DataAnnotations;
namespace Car_rental_system.Helpers.Validation
{
    public class DOFValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DateTime dateOfBirth = (DateTime)value;

            if (dateOfBirth > DateTime.Today)
                return new ValidationResult("تاريخ ميلاد غير صالح");

            int age = DateTime.Today.Year - dateOfBirth.Year;

            return age >= 18
                ? ValidationResult.Success
                : new ValidationResult("يجب أن يكون عمر المستخدم 18 سنة على الأقل.");
        }

    }
}
