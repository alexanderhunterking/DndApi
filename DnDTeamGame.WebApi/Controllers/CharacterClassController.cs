using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.CharacterClassServices;
using DnDTeamGame.Models.CharacterClassModels;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Data.Entities;

namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterClassController : ControllerBase
    {
        private readonly ICharacterClassService _characterClassService;

        public CharacterClassController(ICharacterClassService characterClassService)
        {
            _characterClassService = characterClassService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacterClass([FromBody] CharacterClassCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _characterClassService.CreateNewCharacterClassAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create Character Class."));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CharacterClassList>), 200)]
        public async Task<IActionResult> GetAllCharacterClasses()
        {
            var characterClasses = await _characterClassService.GetAllCharacterClassesAsync();
            return Ok(characterClasses);
        }

        [HttpGet("{characterClassId:int}")]
        public async Task<IActionResult> GetCharacterClassById([FromRoute] int characterClassId)
        {
            CharacterClassDetail? detail = await _characterClassService.GetCharacterClassByIdAsync(characterClassId);
            return detail is not null 
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacterClass([FromBody] CharacterClassUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _characterClassService.UpdateCharacterClassAsync(request)
                ? Ok("Character class was updated successfully.")
                : BadRequest("Character Class could not be found.");
        }

        [HttpDelete("{characterClassId:int}")]
        public async Task<IActionResult> DeleteCharacterClass([FromRoute] int characterClassId)
        {
            return await _characterClassService.DeleteCharacterClassAsync(characterClassId)
                ? Ok($"Character Class with the ID: {characterClassId} was deleted successfully.")
                : BadRequest($"Character class with the ID: {characterClassId} could not be deleted.");
        }
    }
}
