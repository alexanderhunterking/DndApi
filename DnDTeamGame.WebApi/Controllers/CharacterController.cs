using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.CharacterServices;
using DnDTeamGame.Models.CharacterModels;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Data.Entities;

namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _characterService.CreateCharacterAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create Character."));
        }

        [HttpGet("CharacterByUser/{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<CharacterListItem>), 200)]
        public async Task<IActionResult> GetAllCharacters([FromRoute] int userId)
        {
            var characters = await _characterService.GetAllCharactersAsync(userId);
            if (characters == null || !characters.Any())
            {
                return NotFound("No characters found for the given user.");
            }
            return Ok(characters);
        }

        [HttpGet("{characterId:int}")]
        public async Task<IActionResult> GetCharacterById([FromRoute] int characterId)
        {
            CharacterDetail? detail = await _characterService.GetCharacterByIdAsync(characterId);
            return detail is not null
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacterById([FromBody] CharacterUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _characterService.UpdateCharacterAsync(request)
                ? Ok("Character updated successfully.")
                : BadRequest("Character could not be found.");
        }

        //!Delete api/Charater/5
        [HttpDelete("{characterId:int}")]
        public async Task<IActionResult> DeleteCharacter([FromRoute] int characterId)
        {
            return await _characterService.DeleteCharacterAsync(characterId)
                ? Ok($"Character with the ID: {characterId} was deleted successfully.")
                : BadRequest($"Character with the ID: {characterId} could not be deleted.");
        }
    }
}
