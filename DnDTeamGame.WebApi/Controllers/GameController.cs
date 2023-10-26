using DnDTeamGame.Models.Games;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Services.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        //httpost for create game
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateGame([FromBody] GameCreate request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            var response = await _gameService.CreateGameAsync(request);
            if (response is not null)
                return Ok(response);

            return BadRequest(new TextResponse("Could not create game, please try again"));
        }

        //httpget get game by id
        [HttpGet("{gameId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGamesById([FromRoute] int gameId)
        {
            GameDetails? details = await _gameService.GetGameByIdAsync(gameId);

            return details is not null
                ? Ok(details)
                : NotFound();
        }

        //httpget get all games
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GamesListItem>), 200)]

        public async Task<IActionResult> GetAllGames(){
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }


        //httpPut update games by Id  
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateGamesById([FromBody] GameUpdate request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return await _gameService.UpdateGamesAsync(request)
                ? Ok("The Game was updated successfully.")
                : BadRequest("The Game was not updated, please try again.");
        }

        [HttpDelete("{gameId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteGameById([FromRoute] int gameId)
        {
            return await _gameService.DeleteDameAsync(gameId)
                ? Ok($"The Game with Id: {gameId} was successfully deleted.")
                : BadRequest($"The Game with Id: {gameId} was not able to be deleted.");
        }

    }
}
