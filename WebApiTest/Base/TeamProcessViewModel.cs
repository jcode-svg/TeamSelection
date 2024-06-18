// /////////////////////////////////////////////////////////////////////////////
// TESTING AREA
// THIS IS AN AREA WHERE YOU CAN TEST YOUR WORK AND WRITE YOUR TESTS
// /////////////////////////////////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace WebApiTest.Base
{
    public class TeamProcessViewModel
    {
        [Required]
        public string Position { get; set; }
        [Required]
        public string MainSkill { get; set; }
        [Required]
        public string NumberOfPlayers { get; set; }
    }
}
