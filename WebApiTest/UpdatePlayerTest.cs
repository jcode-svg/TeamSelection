using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DTOs.Request;
using WebApi.DTOs.Response;
using WebApi.Entities;

namespace WebApiTest
{
    [TestFixture]
    public class UpdatePlayerTest : BaseTestWrapper
    {
        private int playerId;
        private UpdatePlayerRequest request;
        private Player existingPlayer;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();

            playerId = 1;

            // Setup a sample request
            request = new UpdatePlayerRequest
            {
                Name = "Updated Name",
                Position = "Updated Position",
                PlayerSkills = new List<PlayerSkillDTO> { new PlayerSkillDTO { Skill = "speed", Value = 85 } }
            };

            // Setup an existing player to be returned by the mock repository
            existingPlayer = new Player
            {
                Id = playerId,
                Name = "Old Name",
                Position = "Old Position",
                PlayerSkills = new List<PlayerSkill> { new PlayerSkill { Skill = "attack", Value = 70 } }
            };

            _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                 .ReturnsAsync(existingPlayer);
        }

        [Test]
        public async Task UpdatePlayer_ShouldReturnErrorResponse_WhenPlayerDoesNotExist()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                 .ReturnsAsync((Player)null);

            // Act
            var responseWrapper = await _playerService.UpdatePlayer(playerId, request);

            // Assert
            Assert.IsFalse(responseWrapper.IsSuccessful);
            Assert.AreEqual($"Player with Id {playerId} does not exist", responseWrapper.Message);
        }

        [Test]
        public async Task UpdatePlayer_ShouldUpdatePlayerSuccessfully_WhenPlayerExists()
        {
            // Act
            var responseWrapper = await _playerService.UpdatePlayer(playerId, request);

            // Assert
            Assert.IsTrue(responseWrapper.IsSuccessful);
            Assert.AreEqual(playerId, responseWrapper.ResponseObject.Id);
            Assert.AreEqual("Updated Name", responseWrapper.ResponseObject.Name);
            Assert.AreEqual("Updated Position", responseWrapper.ResponseObject.Position);
            Assert.AreEqual(1, responseWrapper.ResponseObject.PlayerSkills.Count);
            Assert.AreEqual("speed", responseWrapper.ResponseObject.PlayerSkills.First().Skill);

            _playerRepositoryMock.Verify(repo => repo.GetPlayerByIdAsync(playerId), Times.Once);
            _playerRepositoryMock.Verify(repo => repo.ClearPlayerSkills(It.IsAny<Player>()), Times.Once);
            _playerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}
