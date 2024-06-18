using System.ComponentModel.DataAnnotations;
using WebApi.CustomValidationExceptions;
using WebApi.DTOs.Request;

namespace WebApi.CustomValidationAttributes
{

    public class UniquePositionAndSkillAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var requests = value as List<TeamSelectionRequest>;
            if (requests == null)
            {
                return new ValidationResult("Invalid request format.");
            }

            var duplicates = requests.GroupBy(r => new { r.Position, r.MainSkill })
                                     .Where(g => g.Count() > 1)
                                     .Select(g => g.Key)
                                     .ToList();

            if (duplicates.Any())
            {
                var firstDuplicate = duplicates.First();
                var message = $"Duplicate combination of Position: {firstDuplicate.Position} and MainSkill: {firstDuplicate.MainSkill} found.";
                throw new CustomValidationException(message);
            }

            return ValidationResult.Success;
        }
    }
}
