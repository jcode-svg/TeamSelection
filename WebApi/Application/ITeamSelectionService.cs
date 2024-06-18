using WebApi.DTOs.Request;
using WebApi.DTOs.Response;

namespace WebApi.Application
{
    public interface ITeamSelectionService
    {
        Task<ResponseWrapper<IEnumerable<TeamSelectionResponse>>> SelectTeam(List<TeamSelectionRequest> request);
    }
}
