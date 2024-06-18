using WebApi.Application;
using WebApi.Repository;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ITeamSelectionService, TeamSelectionService>();
        }
    }
}
