

using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Application;
using WebApi.CustomValidationAttributes;
using WebApi.DTOs.Request;
using WebApi.DTOs.Response;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/team")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamSelectionService _teamSelectionService;

        public TeamController(ITeamSelectionService teamSelectionService)
        {
            _teamSelectionService = teamSelectionService;
        }

        [HttpPost("process")]
        [ProducesResponseType(typeof(IEnumerable<TeamSelectionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<TeamSelectionResponse>>> Process([UniquePositionAndSkillAttribute] List<TeamSelectionRequest> request)
        {
            var selectTeamrResponse = await _teamSelectionService.SelectTeam(request);

            if (!selectTeamrResponse.IsSuccessful)
            {
                return BadRequest(new ErrorResponse(selectTeamrResponse.Message));
            }

            return Ok(selectTeamrResponse.ResponseObject);
        }
    }
}
