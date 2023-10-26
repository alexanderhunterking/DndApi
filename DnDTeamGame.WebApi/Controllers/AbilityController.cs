using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.AbilityServices;
using DnDTeamGame.Models.AbilityModels;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Data.Entities;

namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AbilityController : ControllerBase
    {
        private readonly IAbilityService _abilityService;

        public AbilityController(IAbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAbility([FromBody] AbilityCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _abilityService.CreateNewAbilityAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create Ability."));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AbilityListItem>), 200)]
        public async Task<IActionResult> GetAllAbilities()
        {
            var abilities = await _abilityService.GetAllAbilitiesAsync();
            return Ok(abilities);
        }

        [HttpGet("{abilityId:int}")]
        public async Task<IActionResult> GetAbilityById([FromRoute] int abilityId)
        {
            AbilityDetail? detail = await _abilityService.GetAbilityByIdAsync(abilityId);
            return detail is not null 
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAbility([FromBody] AbilityUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _abilityService.UpdateAbilityAsync(request)
                ? Ok("Ability updated successfully.")
                : BadRequest("Ability could not be found.");
        }

        //!Delete api/Ability/5
        [HttpDelete("{abilityId:int}")]
        public async Task<IActionResult> DeleteAbility([FromRoute] int abilityId)
        {
            return await _abilityService.DeleteAbilityAsync(abilityId)
                ? Ok($"Ability with the ID: {abilityId} was deleted successfully.")
                : BadRequest($"Ability with the ID: {abilityId} could not be deleted.");
        }
    }
}
