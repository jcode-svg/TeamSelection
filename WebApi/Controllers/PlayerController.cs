

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Entities;
using WebApi.DTOs.Response;
using WebApi.Application;
using WebApi.DTOs.Request;
using System.Net.Mime;

[ApiController]
[Route("api/player")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatePlayerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<CreatePlayerResponse>> CreatePlayer(CreatePlayerRequest request)
    {
        var createPlayerResponse = await _playerService.AddPlayer(request);

        if (!createPlayerResponse.IsSuccessful)
        {
            return BadRequest(new ErrorResponse(createPlayerResponse.Message));
        }

        return Ok(createPlayerResponse.ResponseObject);
    }

    [HttpPut("{playerId}")]
    [ProducesResponseType(typeof(UpdatePlayerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<UpdatePlayerResponse>> UpdatePlayer(int playerId, UpdatePlayerRequest request)
    {
        var updatePlayerResponse = await _playerService.UpdatePlayer(playerId, request);

        if (!updatePlayerResponse.IsSuccessful)
        {
            return BadRequest(new ErrorResponse(updatePlayerResponse.Message));
        }

        return Ok(updatePlayerResponse.ResponseObject);
    }

    [HttpDelete("{playerId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<string>> DeletePlayer(int playerId, [FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Unauthorized(new ErrorResponse("Invalid authorization header format."));
        }

        string token = authorizationHeader.Substring("Bearer ".Length).Trim();

        var deletePlayerResponse = await _playerService.DeletePlayer(playerId, token);

        if (deletePlayerResponse.StatusCode == 401)
        {
            return Unauthorized(new ErrorResponse(deletePlayerResponse.Message));
        }

        if (!deletePlayerResponse.IsSuccessful)
        {
            return BadRequest(new ErrorResponse(deletePlayerResponse.Message));
        }

        return Ok(deletePlayerResponse.ResponseObject);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlayerDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<PlayerDTO>>> AllPlayers()
    {
        var allPlayerResponse = await _playerService.GetAllPlayers();

        if (!allPlayerResponse.IsSuccessful)
        {
            return BadRequest(new ErrorResponse(allPlayerResponse.Message));
        }

        return Ok(allPlayerResponse.ResponseObject);
    }
}