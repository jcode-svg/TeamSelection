﻿// /////////////////////////////////////////////////////////////////////////////
// PLEASE DO NOT RENAME OR REMOVE ANY OF THE CODE BELOW. 
// YOU CAN ADD YOUR CODE TO THIS FILE TO EXTEND THE FEATURES TO USE THEM IN YOUR WORK.
// /////////////////////////////////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.CustomValidationAttributes;

namespace WebApi.Entities
{
    public class PlayerSkill
    {
        [Key]
        public int Id { get; set; }
        public string Skill { get; set; }
        [Range(0, 100)]
        public int Value { get; set; }
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }
    }
}
