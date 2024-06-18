using System.ComponentModel.DataAnnotations;
using WebApi.CustomValidationAttributes;

namespace WebApi.DTOs.Response
{
    public class PlayerSkillDTO
    {
        [ValidSkill]
        public string Skill { get; set; }
        [Range(0, 100)]
        public int Value { get; set; }
    }
}
