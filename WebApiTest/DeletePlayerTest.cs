// /////////////////////////////////////////////////////////////////////////////
// TESTING AREA
// THIS IS AN AREA WHERE YOU CAN TEST YOUR WORK AND WRITE YOUR TESTS
// /////////////////////////////////////////////////////////////////////////////

using Moq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApiTest
{
    [TestFixture]
    public class DeletePlayerTest : BaseTestWrapper
    {
        private int playerId;
        private Player player;
        private string validToken;
        private string invalidToken;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();
            playerId = 1;
            player = new Player
            {
                Id = playerId,
                Name = "player name",
                Position = "defender"
            };
            validToken = "SkFabTZibXE1aE14ckpQUUxHc2dnQ2RzdlFRTTM2NFE2cGI4d3RQNjZmdEFITmdBQkE=";
            invalidToken = "invalid";
        }

        [Test]
        public async Task TestSample()
        {
            var response = await client.DeleteAsync($"/api/player/{playerId}");
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
        public async Task DeletePlayer_ShouldReturnSuccessResponse_WhenPlayerIsDeleted()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                 .ReturnsAsync(player);
            _playerRepositoryMock.Setup(repo => repo.RemovePlayer(player));
            _playerRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                                 .Returns(Task.FromResult(1));

            // Act
            var responseWrapper = await _playerService.DeletePlayer(playerId, validToken);

            // Assert
            Assert.IsTrue(responseWrapper.IsSuccessful);
            Assert.AreEqual("Player deleted successfully", responseWrapper.ResponseObject);
            _playerRepositoryMock.Verify(repo => repo.GetPlayerByIdAsync(playerId), Times.Once);
            _playerRepositoryMock.Verify(repo => repo.RemovePlayer(player), Times.Once);
            _playerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeletePlayer_ShouldReturnErrorResponse_WhenPlayerDoesNotExist()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                 .ReturnsAsync((Player)null);

            // Act
            var responseWrapper = await _playerService.DeletePlayer(playerId, validToken);

            // Assert
            Assert.IsFalse(responseWrapper.IsSuccessful);
            Assert.AreEqual($"Player with Id {playerId} does not exist", responseWrapper.Message);
            _playerRepositoryMock.Verify(repo => repo.GetPlayerByIdAsync(playerId), Times.Once);
            _playerRepositoryMock.Verify(repo => repo.RemovePlayer(It.IsAny<Player>()), Times.Never);
            _playerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task DeletePlayer_ShouldReturnUnauthorizedResponse_WhenTokenIsInvalid()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                 .ReturnsAsync(player);

            // Act
            var responseWrapper = await _playerService.DeletePlayer(playerId, invalidToken);

            // Assert
            Assert.IsFalse(responseWrapper.IsSuccessful);
            Assert.AreEqual("Invalid token.", responseWrapper.Message);
            Assert.AreEqual(401, responseWrapper.StatusCode);
            _playerRepositoryMock.Verify(repo => repo.GetPlayerByIdAsync(It.IsAny<int>()), Times.Never);
            _playerRepositoryMock.Verify(repo => repo.RemovePlayer(It.IsAny<Player>()), Times.Never);
            _playerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task DeletePlayer_ShouldReturnErrorResponse_WhenPlayerDoesNotExist_WithValidToken()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetPlayerByIdAsync(playerId))
                                 .ReturnsAsync((Player)null);

            // Act
            var responseWrapper = await _playerService.DeletePlayer(playerId, validToken);

            // Assert
            Assert.IsFalse(responseWrapper.IsSuccessful);
            Assert.AreEqual($"Player with Id {playerId} does not exist", responseWrapper.Message);
            Assert.AreEqual(400, responseWrapper.StatusCode);
            _playerRepositoryMock.Verify(repo => repo.GetPlayerByIdAsync(playerId), Times.Once);
            _playerRepositoryMock.Verify(repo => repo.RemovePlayer(It.IsAny<Player>()), Times.Never);
            _playerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}
