using WebApi.DTOs.Request;
using WebApi.DTOs.Response;
using WebApi.Entities;

namespace WebApi.Application
{
    public interface IPlayerService
    {
        Task<ResponseWrapper<CreatePlayerResponse>> AddPlayer(CreatePlayerRequest request);
        Task<ResponseWrapper<string>> DeletePlayer(int playerId, string token);
        Task<ResponseWrapper<IEnumerable<PlayerDTO>>> GetAllPlayers();
        Task<ResponseWrapper<UpdatePlayerResponse>> UpdatePlayer(int playerId, UpdatePlayerRequest request);
    }
}
