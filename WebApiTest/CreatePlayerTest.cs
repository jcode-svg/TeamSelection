// /////////////////////////////////////////////////////////////////////////////
// TESTING AREA
// THIS IS AN AREA WHERE YOU CAN TEST YOUR WORK AND WRITE YOUR TESTS
// /////////////////////////////////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApi.Application;
using WebApi.DTOs.Request;
using WebApi.DTOs.Response;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Repository;
using WebApiTest.Base;

namespace WebApiTest
{
    [TestFixture]
    public class CreatePlayerTest : BaseTestWrapper
    {
        private PlayerSkillDTO requestSkill1;
        private PlayerSkillDTO requestSkill2;
        private CreatePlayerRequest request;
        private Player newPlayer;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();

            // Setup request data
            requestSkill1 = new PlayerSkillDTO { Skill = "attack", Value = 60 };
            requestSkill2 = new PlayerSkillDTO { Skill = "speed", Value = 80 };

            request = new CreatePlayerRequest
            {
                Name = "player name",
                Position = "defender",
                PlayerSkills = new List<PlayerSkillDTO> { requestSkill1, requestSkill2 }
            };

            // Setup mock player creation
            newPlayer = new Player
            {
                Id = 1,
                Name = request.Name,
                Position = request.Position,
                PlayerSkills = request.PlayerSkills.Select(p => new PlayerSkill
                {
                    Id = 1,
                    Skill = p.Skill,
                    Value = p.Value,
                    PlayerId = 1
                }).ToList()
            };

            _playerRepositoryMock.Setup(repo => repo.CreatePlayer(It.IsAny<Player>()))
                                 .ReturnsAsync(newPlayer);
            _playerRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                                 .Returns(Task.FromResult(1));
        }

        [Test]
        public async Task AddPlayer_ShouldReturnSuccessResponse_WhenPlayerIsCreated()
        {
            // Act
            var responseWrapper = await _playerService.AddPlayer(request);

            // Assert
            Assert.IsTrue(responseWrapper.IsSuccessful);
            Assert.AreEqual(responseWrapper.ResponseObject.Id, newPlayer.Id);
            Assert.AreEqual(responseWrapper.ResponseObject.Name, newPlayer.Name);
            Assert.AreEqual(responseWrapper.ResponseObject.Position, newPlayer.Position);
            Assert.AreEqual(responseWrapper.ResponseObject.PlayerSkills.Count, newPlayer.PlayerSkills.Count);
            _playerRepositoryMock.Verify(repo => repo.CreatePlayer(It.IsAny<Player>()), Times.Once);
            _playerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task TestSample()
        {
            var player = new Player
            {
                Name = "player name",
                Position = "defender",
                PlayerSkills = new List<PlayerSkill>
            {
                new PlayerSkill { Skill = "attack", Value = 60 },
                new PlayerSkill { Skill = "speed", Value = 80 },
            }
            };

            var response = await client.PostAsJsonAsync("/api/player", player);

            var responseObject = await response.Content.ReadAsStringAsync();
            Assert.That(responseObject, Is.Not.Null);
        }
    }
}
