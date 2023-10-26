using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.MapServices;
using DnDTeamGame.Data;
using Microsoft.AspNetCore.Authorization;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Models.MapModels;

namespace DnDTeamGame.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapGenerator _mapGenerator;

        public MapController(IMapGenerator mapGenerator)
        {
            _mapGenerator = mapGenerator;

        }

        [HttpPost]
        public async Task<IActionResult> CreateMap([FromBody] MapCreate request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _mapGenerator.CreatMapAsync(request);
            if (response is not null)
                return Ok(response);
            return BadRequest(new TextResponse("Could not create map"));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMaps()
        {
            var maps = await _mapGenerator.GetAllMapsAsync();
            return Ok(maps);
        }
        [HttpGet("{noteId:int}")]
        public async Task<IActionResult> GetNoteById([FromRoute] int noteId)
        {
            MapDetail? detail = await _mapGenerator.GetMapByGameIdAsync(noteId);

            return detail is not null
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNoteById([FromBody] MapUpdate request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await _mapGenerator.UpdateMapAsync(request)
            ? Ok("Note updated successfully.")
            : BadRequest("Note could not be updated.");
        }
        [HttpDelete("{gameId:int}")]
        public async Task<IActionResult> DeleteMap([FromRoute] int gameId)
        {
            return await _mapGenerator.DeleteMapAsync(gameId)
            ? Ok($"Map {gameId} was deleted successfully.")
            : BadRequest($"Map {gameId} could not be deleted.");
        }

    }
}
