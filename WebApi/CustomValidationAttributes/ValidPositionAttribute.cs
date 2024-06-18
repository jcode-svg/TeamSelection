using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WebApi.CustomValidationExceptions;

namespace WebApi.CustomValidationAttributes
{
    public class ValidPositionAttribute : ValidationAttribute
    {
        private readonly string _pattern = "^(defender|midfielder|forward)$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string position && !Regex.IsMatch(position, _pattern))
            {
                throw new CustomValidationException($"Invalid value for position: {position}");
            }

            return ValidationResult.Success;
        }
    }
}
