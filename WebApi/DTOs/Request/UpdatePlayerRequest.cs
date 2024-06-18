using System.ComponentModel.DataAnnotations;
using WebApi.CustomValidationAttributes;
using WebApi.DTOs.Response;
using WebApi.Entities;

namespace WebApi.DTOs.Request
{
    public class UpdatePlayerRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [ValidPosition]
        public string Position { get; set; }

        [Required(ErrorMessage = "PlayerSkills is required")]
        public List<PlayerSkillDTO> PlayerSkills { get; set; }
    }
}
