using WebApi.Entities;

namespace WebApi.DTOs.Response
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<PlayerSkill> PlayerSkills { get; set; }
    }
}
