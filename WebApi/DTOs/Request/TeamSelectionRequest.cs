using System.ComponentModel.DataAnnotations;
using WebApi.CustomValidationAttributes;

namespace WebApi.DTOs.Request
{
    public class SelectTeamRequest
    {
        [Required(ErrorMessage = "Team Selection Request is required")]
        [UniquePositionAndSkill]
        public List<TeamSelectionRequest> TeamSelectionRequests { get; set; }
    }

    public class TeamSelectionRequest
    {
        [Required(ErrorMessage = "Position is required")]
        [ValidPosition]
        public string Position { get; set; }

        [Required(ErrorMessage = "Main Skill is required")]
        [ValidSkill]
        public string MainSkill { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Number Of Players must be greater than 0")]
        public int NumberOfPlayers { get; set; }
    }
}
