using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.ArmourServices;
using DnDTeamGame.Models.ArmourModels;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Data.Entities;
namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArmourController : ControllerBase
    {
        private readonly IArmourService _armourService;

        public ArmourController(IArmourService armourService)
        {
            _armourService = armourService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArmour([FromBody] ArmourCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _armourService.CreateNewArmourAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create Armour."));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ArmourList>), 200)]
        public async Task<IActionResult> GetAllArmours()
        {
            var armours = await _armourService.GetAllArmoursAsync();
            return Ok(armours);
        }

        [HttpGet("{armourId:int}")]
        public async Task<IActionResult> GetArmourById([FromRoute] int armourId)
        {
            ArmourDetail? detail = await _armourService.GetArmourByIdAsync(armourId);
            return detail is not null 
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArmour([FromBody] ArmourUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _armourService.UpdateArmourAsync(request)
                ? Ok("Armour updated successfully.")
                : BadRequest("Armour could not be found.");
        }

        [HttpDelete("{armourId:int}")]
        public async Task<IActionResult> DeleteArmour([FromRoute] int armourId)
        {
            return await _armourService.DeleteArmourAsync(armourId)
                ? Ok($"Armour with the ID: {armourId} was deleted successfully.")
                : BadRequest($"Armour with the ID: {armourId} could not be deleted.");
        }
    }
}
