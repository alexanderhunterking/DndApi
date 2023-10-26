using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.HairColorServices;
using DnDTeamGame.Models.HairColorModels;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Data.Entities;

namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HairColorController : ControllerBase
    {
        private readonly IHairColorService _hairColorService;

        public HairColorController(IHairColorService hairColorService)
        {
            _hairColorService = hairColorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHairColor([FromBody] HairColorCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _hairColorService.CreateNewHairColorAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create HairColor."));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HairColorList>), 200)]
        public async Task<IActionResult> GetAllHairColors()
        {
            var hairColors = await _hairColorService.GetAllHairColorsAsync();
            return Ok(hairColors);
        }

        [HttpGet("{hairColorId:int}")]
        public async Task<IActionResult> GetHairColorById([FromRoute] int hairColorId)
        {
            HairColorDetail? detail = await _hairColorService.GetHairColorByIdAsync(hairColorId);
            return detail is not null 
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHairColor([FromBody] HairColorUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _hairColorService.UpdateHairColorAsync(request)
                ? Ok("HairColor updated successfully.")
                : BadRequest("HairColor could not be found.");
        }

        [HttpDelete("{hairColorId:int}")]
        public async Task<IActionResult> DeleteHairColor([FromRoute] int hairColorId)
        {
            return await _hairColorService.DeleteHairColorAsync(hairColorId)
                ? Ok($"HairColor with the ID: {hairColorId} was deleted successfully.")
                : BadRequest($"HairColor with the ID: {hairColorId} could not be deleted.");
        }
    }
}
