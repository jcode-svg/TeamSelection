// /////////////////////////////////////////////////////////////////////////////
// TESTING AREA
// THIS IS AN AREA WHERE YOU CAN TEST YOUR WORK AND WRITE YOUR TESTS
// /////////////////////////////////////////////////////////////////////////////

using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApiTest
{
    [TestFixture]
    public class ListPlayerTest : BaseTestWrapper
    {
        private List<Player> players;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();
            players = new List<Player>
        {
            new Player { Id = 1, Name = "Player1", Position = "Forward", PlayerSkills = new List<PlayerSkill> { new PlayerSkill { Skill = "Speed", Value = 95 } } },
            new Player { Id = 2, Name = "Player2", Position = "Midfielder", PlayerSkills = new List<PlayerSkill> { new PlayerSkill { Skill = "Strength", Value = 85 } } }
        };
        }

        [Test]
        public async Task TestSample()
        {
            var response = await client.GetAsync("/api/player");
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
        public async Task GetAllPlayers_ShouldReturnSuccessResponse_WhenPlayersExist()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetAllPlayersAsync())
                                 .ReturnsAsync(players);

            // Act
            var responseWrapper = await _playerService.GetAllPlayers();

            // Assert
            Assert.IsTrue(responseWrapper.IsSuccessful);
            Assert.AreEqual(2, responseWrapper.ResponseObject.Count());
            _playerRepositoryMock.Verify(repo => repo.GetAllPlayersAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllPlayers_ShouldReturnErrorResponse_WhenNoPlayersExist()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetAllPlayersAsync())
                                 .ReturnsAsync(new List<Player>());

            // Act
            var responseWrapper = await _playerService.GetAllPlayers();

            // Assert
            Assert.IsFalse(responseWrapper.IsSuccessful);
            Assert.AreEqual("There are no players available", responseWrapper.Message);
            _playerRepositoryMock.Verify(repo => repo.GetAllPlayersAsync(), Times.Once);
        }
    }
}
