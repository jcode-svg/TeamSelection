using WebApi.DTOs.Request;
using WebApi.DTOs.Response;
using WebApi.Entities;
using WebApi.Repository;

namespace WebApi.Application
{
    public class TeamSelectionService : ITeamSelectionService
    {
        private readonly IPlayerRepository _playerRepository;

        public TeamSelectionService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<ResponseWrapper<IEnumerable<TeamSelectionResponse>>> SelectTeam(List<TeamSelectionRequest> request)
        {
            var selectedPlayers = new List<Player>();
            var usedPlayerIds = new HashSet<int>();

            foreach (var requirement in request)
            {
                var players = await _playerRepository.SelectPlayersByRequirementsAsync(requirement, usedPlayerIds);

                if (!ValidateSuffictentPlayers(players, requirement))
                {
                    return ResponseWrapper<IEnumerable<TeamSelectionResponse>>.Error($"Insufficient number of players for position: {requirement.Position}");
                }

                selectedPlayers.AddRange(players);
                foreach (var player in players)
                {
                    usedPlayerIds.Add(player.Id);
                }
            }

            var selectedPlayersDTO = selectedPlayers.Select(p => new TeamSelectionResponse
            {
                Name = p.Name,
                Position = p.Position,
                PlayerSkills = p.PlayerSkills
            });

            return ResponseWrapper<IEnumerable<TeamSelectionResponse>>.Success(selectedPlayersDTO);
        }

        private bool ValidateSuffictentPlayers(IEnumerable<Player> players, TeamSelectionRequest requirement)
        {
            return players.Count() >= requirement.NumberOfPlayers;
        }
    }
}
