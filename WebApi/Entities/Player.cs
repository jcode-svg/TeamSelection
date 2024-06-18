// /////////////////////////////////////////////////////////////////////////////
// PLEASE DO NOT RENAME OR REMOVE ANY OF THE CODE BELOW. 
// YOU CAN ADD YOUR CODE TO THIS FILE TO EXTEND THE FEATURES TO USE THEM IN YOUR WORK.
// /////////////////////////////////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using WebApi.DTOs.Request;

namespace WebApi.Entities;

public class Player
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public List<PlayerSkill> PlayerSkills { get; set; }

    public static Player CreateNewPlayer(CreatePlayerRequest request)
    {
        return new Player
        {
            Name = request.Name,
            Position = request.Position,
            PlayerSkills = request.PlayerSkills.Select(ps => new PlayerSkill
            {
                Skill = ps.Skill,
                Value = ps.Value
            }).ToList()
        };
    }

    public void UpdatePlayer(UpdatePlayerRequest request)
    {
        Name = request.Name;
        Position = request.Position;

        PlayerSkills.Clear();

        PlayerSkills = request.PlayerSkills.Select(ps => new PlayerSkill
        {
            Skill = ps.Skill,
            Value = ps.Value
        }).ToList();
    }
}