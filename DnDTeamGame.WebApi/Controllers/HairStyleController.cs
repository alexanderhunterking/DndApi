using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.HairStyleServices;
using DnDTeamGame.Models.HairStyleModels;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Data.Entities;

namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HairStyleController : ControllerBase
    {
        private readonly IHairStyleService _hairStyleService;

        public HairStyleController(IHairStyleService hairStyleService)
        {
            _hairStyleService = hairStyleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHairStyle([FromBody] HairStyleCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _hairStyleService.CreateNewHairStyleAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create HairStyle."));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HairStyleList>), 200)]
        public async Task<IActionResult> GetAllHairStyles()
        {
            var hairStyles = await _hairStyleService.GetAllHairStylesAsync();
            return Ok(hairStyles);
        }

        [HttpGet("{hairStyleId:int}")]
        public async Task<IActionResult> GetHairStyleById([FromRoute] int hairStyleId)
        {
            HairStyleDetail? detail = await _hairStyleService.GetHairStyleByIdAsync(hairStyleId);
            return detail is not null 
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHairStyle([FromBody] HairStyleUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _hairStyleService.UpdateHairStyleAsync(request)
                ? Ok("HairStyle updated successfully.")
                : BadRequest("HairStyle could not be found.");
        }

        [HttpDelete("{hairStyleId:int}")]
        public async Task<IActionResult> DeleteHairStyle([FromRoute] int hairStyleId)
        {
            return await _hairStyleService.DeleteHairStyleAsync(hairStyleId)
                ? Ok($"HairStyle with the ID: {hairStyleId} was deleted successfully.")
                : BadRequest($"HairStyle with the ID: {hairStyleId} could not be deleted.");
        }
    }
}
