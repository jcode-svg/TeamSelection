using WebApi.Entities;

namespace WebApi.DTOs.Response
{
    public class TeamSelectionResponse
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public List<PlayerSkill> PlayerSkills { get; set; }
    }
}
