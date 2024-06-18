// /////////////////////////////////////////////////////////////////////////////
// TESTING AREA
// THIS IS AN AREA WHERE YOU CAN TEST YOUR WORK AND WRITE YOUR TESTS
// /////////////////////////////////////////////////////////////////////////////

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApi.Application;
using WebApi.ConfigurationModels;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Repository;
using WebApiTest.Base;

namespace WebApiTest
{

    public abstract class BaseTestWrapper
    {
        protected HttpClient client;
        protected DataContext dataContext;
        protected Mock<IPlayerRepository> _playerRepositoryMock;
        protected PlayerService _playerService;
        protected TeamSelectionService _teamService;

        [SetUp]
        public virtual async Task Setup()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;

            dataContext = new DataContext(contextOptions);
            var app = new TestApplication(_connection);

            client = app.CreateClient();

            var mockConfig = new Mock<IOptions<AppSetting>>();
            mockConfig.Setup(o => o.Value).Returns(new AppSetting { Token = "SkFabTZibXE1aE14ckpQUUxHc2dnQ2RzdlFRTTM2NFE2cGI4d3RQNjZmdEFITmdBQkE=" });

            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _playerService = new PlayerService(_playerRepositoryMock.Object, mockConfig.Object);
            _teamService = new TeamSelectionService(_playerRepositoryMock.Object);

            await dataContext.Database.EnsureDeletedAsync();
            await dataContext.Database.EnsureCreatedAsync();
        }


        [TearDown]
        public virtual void TearDown()
        {
            dataContext.Database.EnsureDeleted();
            client.Dispose();
        }

        protected async Task CreatePlayer(PlayerViewModel newPlayer = null)
        {
            newPlayer ??= playerOne;
            Player model = new()
            {
                Name = newPlayer.Name,
                Position = newPlayer.Position,
                PlayerSkills = newPlayer.PlayerSkills.Select(x => new PlayerSkill
                {
                    Skill = x.Skill,
                    Value = x.Value
                }).ToList()
            };

            dataContext.Players.Add(model);
            await dataContext.SaveChangesAsync();
        }

        protected PlayerViewModel playerOne = new()
        {
            Name = "player name",
            Position = "defender",
            PlayerSkills = new()
            {
                new() { Skill = "attack", Value = 60 },
                new() { Skill = "speed", Value = 80 },
            }
        };
    }
}
