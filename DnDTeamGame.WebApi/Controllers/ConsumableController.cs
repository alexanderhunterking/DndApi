using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DnDTeamGame.Services.ConsumableServices;
using DnDTeamGame.Models.ConsumableModels;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Data.Entities;

namespace DnDTeamGame.WebApi.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumableController : ControllerBase
    {
        private readonly IConsumableService _consumableService;

        public ConsumableController(IConsumableService consumableService)
        {
            _consumableService = consumableService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConsumable([FromBody] ConsumableCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _consumableService.CreateNewConsumableAsync(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return BadRequest(new TextResponse("Could not create Consumable."));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ConsumableList>), 200)]
        public async Task<IActionResult> GetAllConsumables()
        {
            var consumables = await _consumableService.GetAllConsumablesAsync();
            return Ok(consumables);
        }

        [HttpGet("{consumableId:int}")]
        public async Task<IActionResult> GetConsumableById([FromRoute] int consumableId)
        {
            ConsumableDetail? detail = await _consumableService.GetConsumableByIdAsync(consumableId);
            return detail is not null 
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateConsumable([FromBody] ConsumableUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _consumableService.UpdateConsumableAsync(request)
                ? Ok("Consumable updated successfully.")
                : BadRequest("Consumable could not be found.");
        }

        [HttpDelete("{consumableId:int}")]
        public async Task<IActionResult> DeleteConsumable([FromRoute] int consumableId)
        {
            return await _consumableService.DeleteConsumableAsync(consumableId)
                ? Ok($"Consumable with the ID: {consumableId} was deleted successfully.")
                : BadRequest($"Consumable with the ID: {consumableId} could not be deleted.");
        }
    }
}
