using Microsoft.Extensions.Options;
using WebApi.ConfigurationModels;
using WebApi.DTOs.Request;
using WebApi.DTOs.Response;
using WebApi.Entities;
using WebApi.Repository;

namespace WebApi.Application
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly AppSetting _config;

        public PlayerService(IPlayerRepository playerRepository, IOptions<AppSetting> config)
        {
            _playerRepository = playerRepository;
            _config = config.Value;
        }

        public async Task<ResponseWrapper<CreatePlayerResponse>> AddPlayer(CreatePlayerRequest request)
        {
            var newPlayer = Player.CreateNewPlayer(request);

            var player = await _playerRepository.CreatePlayer(newPlayer);
            await _playerRepository.SaveChangesAsync();

            var response = new CreatePlayerResponse
            {
                Id = player.Id,
                Name = player.Name,
                Position = player.Position,
                PlayerSkills = player.PlayerSkills
            };

            return ResponseWrapper<CreatePlayerResponse>.Success(response);
        }

        public async Task<ResponseWrapper<UpdatePlayerResponse>> UpdatePlayer(int playerId, UpdatePlayerRequest request)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(playerId);

            if (player == null)
            {
                return ResponseWrapper<UpdatePlayerResponse>.Error($"Player with Id {playerId} does not exist");
            }

            //EF Core tracks this entity
            _playerRepository.ClearPlayerSkills(player);
            player.UpdatePlayer(request);
            await _playerRepository.SaveChangesAsync();

            var response = new UpdatePlayerResponse
            {
                Id = player.Id,
                Name = player.Name,
                Position = player.Position,
                PlayerSkills = player.PlayerSkills
            };

            return ResponseWrapper<UpdatePlayerResponse>.Success(response);
        }

        public async Task<ResponseWrapper<string>> DeletePlayer(int playerId, string token)
        {
            bool isTokenValid = ValidateToken(token);

            if (!isTokenValid)
            {
                return ResponseWrapper<string>.Error("Invalid token.", statusCode: 401);
            }

            var player = await _playerRepository.GetPlayerByIdAsync(playerId);

            if (player == null)
            {
                return ResponseWrapper<string>.Error($"Player with Id {playerId} does not exist");
            }

            _playerRepository.RemovePlayer(player);
            await _playerRepository.SaveChangesAsync();

            return ResponseWrapper<string>.Success("Player deleted successfully");
        }

        private bool ValidateToken(string token)
        {
            return token == _config.Token;
        }

        public async Task<ResponseWrapper<IEnumerable<PlayerDTO>>> GetAllPlayers()
        {
            var allPlayers = await _playerRepository.GetAllPlayersAsync();

            if (!allPlayers.Any())
            {
                return ResponseWrapper<IEnumerable<PlayerDTO>>.Error("There are no players available");
            }

            var allPlayersDTO = allPlayers.Select(p => new PlayerDTO
            {
                Id = p.Id,
                Name = p.Name,
                Position = p.Position,
                PlayerSkills = p.PlayerSkills
            });

            return ResponseWrapper<IEnumerable<PlayerDTO>>.Success(allPlayersDTO);
        }
    }
}