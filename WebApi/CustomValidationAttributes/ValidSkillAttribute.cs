using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WebApi.CustomValidationExceptions;

namespace WebApi.CustomValidationAttributes
{
    public class ValidSkillAttribute : ValidationAttribute
    {
        private readonly string _pattern = "^(defense|attack|speed|strength|stamina)$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string skill && !Regex.IsMatch(skill, _pattern))
            {
                throw new CustomValidationException($"Invalid value for skill: {skill}");
            }

            return ValidationResult.Success;
        }
    }
}
