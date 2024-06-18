// /////////////////////////////////////////////////////////////////////////////
// TESTING AREA
// THIS IS AN AREA WHERE YOU CAN TEST YOUR WORK AND WRITE YOUR TESTS
// /////////////////////////////////////////////////////////////////////////////

using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApi.DTOs.Request;
using WebApi.Entities;
using WebApiTest.Base;

namespace WebApiTest
{
    [TestFixture]
    public class ProcessTeamTest : BaseTestWrapper
    {
        private TeamSelectionRequest requirement;
        private List<Player> players;
        private HashSet<int> usedPlayerIds;

        [SetUp]
        public async Task Setup()
        {
            await base.Setup();

            requirement = new TeamSelectionRequest
            {
                Position = "defender",
                MainSkill = "speed",
                NumberOfPlayers = 1
            };

            players = new List<Player>
        {
            new Player { Id = 1, Name = "Player1", Position = "defender", PlayerSkills = new List<PlayerSkill> { new PlayerSkill { Skill = "speed", Value = 90 } } }
        };

            usedPlayerIds = new HashSet<int>();
        }

        [Test]
        public async Task TestSample()
        {
            List<TeamProcessViewModel> requestData = new List<TeamProcessViewModel>()
        {
            new TeamProcessViewModel()
            {
                Position = "defender",
                MainSkill = "speed",
                NumberOfPlayers = "1"
            }
        };

            var response = await client.PostAsJsonAsync("/api/team/process", requestData);
            try
            {
                var responseObject = await response.Content.ReadAsStringAsync();
                Assert.That(responseObject, Is.Not.Null);
            }
            catch
            {
                Assert.Fail("Invalid response object");
            }

        }

        [Test]
        public async Task SelectTeam_ShouldReturnErrorResponse_WhenInsufficientPlayers()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.SelectPlayersByRequirementsAsync(requirement, It.IsAny<HashSet<int>>()))
                                 .ReturnsAsync(new List<Player>());

            // Act
            var responseWrapper = await _teamService.SelectTeam(new List<TeamSelectionRequest> { requirement });

            // Assert
            Assert.IsFalse(responseWrapper.IsSuccessful);
            Assert.AreEqual($"Insufficient number of players for position: {requirement.Position}", responseWrapper.Message);
            _playerRepositoryMock.Verify(repo => repo.SelectPlayersByRequirementsAsync(requirement, It.IsAny<HashSet<int>>()), Times.Once);
        }

        [Test]
        public async Task SelectTeam_ShouldReturnSuccessResponse_WhenSufficientPlayers()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.SelectPlayersByRequirementsAsync(requirement, It.IsAny<HashSet<int>>()))
                                 .ReturnsAsync(players);

            // Act
            var responseWrapper = await _teamService.SelectTeam(new List<TeamSelectionRequest> { requirement });

            // Assert
            Assert.IsTrue(responseWrapper.IsSuccessful);
            Assert.AreEqual(1, responseWrapper.ResponseObject.Count());
            _playerRepositoryMock.Verify(repo => repo.SelectPlayersByRequirementsAsync(requirement, It.IsAny<HashSet<int>>()), Times.Once);
        }
    }
}
