using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Request;
using WebApi.DTOs.Response;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Repository
{
    public class PlayerRepository : RepositoryAbstract, IPlayerRepository
    {
        private readonly DataContext _applicationDbContext;

        public PlayerRepository(DataContext dbContext) : base(dbContext)
        {
            _applicationDbContext = dbContext;
        }

        public async Task<Player> CreatePlayer(Player newPlayer)
        {
            var newRecord = await _applicationDbContext.Players.AddAsync(newPlayer);
            return newRecord.Entity;
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _applicationDbContext.Players
                .Include(p => p.PlayerSkills)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Player RemovePlayer(Player player)
        {
            var record = _applicationDbContext.Players.Remove(player);
            return record.Entity;
        }

        public void ClearPlayerSkills(Player player)
        {
            _applicationDbContext.PlayerSkills.RemoveRange(player.PlayerSkills);
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _applicationDbContext.Players.Include(p => p.PlayerSkills).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Player>> SelectPlayersByRequirementsAsync(TeamSelectionRequest request, HashSet<int> usedPlayerIds)
        {
            return await _applicationDbContext.Players
                .Include (p => p.PlayerSkills)
                .Where(p => p.Position == request.Position && !usedPlayerIds.Contains(p.Id))
                .OrderByDescending(p => p.PlayerSkills
                .Where(s => s.Skill == request.MainSkill)
                .Max(s => (int?)s.Value) ?? 0)
                .ThenByDescending(p => p.PlayerSkills.Max(s => s.Value))
                .Take(request.NumberOfPlayers)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _applicationDbContext.SaveChangesAsync(new CancellationToken());
        }
    }
}
