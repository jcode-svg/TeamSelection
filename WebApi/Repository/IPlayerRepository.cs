using WebApi.DTOs.Request;
using WebApi.Entities;

namespace WebApi.Repository
{
    public interface IPlayerRepository
    {
        void ClearPlayerSkills(Player player);
        Task<Player> CreatePlayer(Player newPlayer);
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int id);
        Player RemovePlayer(Player player);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<Player>> SelectPlayersByRequirementsAsync(TeamSelectionRequest request, HashSet<int> usedPlayerIds);
    }
}
