using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DnDTeamGame.Models.Responses;
using DnDTeamGame.Models.VehicleModels;
using DnDTeamGame.Services.VehicleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DnDTeamGame.WebApi.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VehicleController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] VehicleCreate request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _vehicleService.CreateVehicleAsync(request);
        if (response is not null)
            return Ok(response);

        return BadRequest(new TextResponse("Could not create vehicle! Please try again!"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVehicles()
    {
        var vehicles = await _vehicleService.GetAllVehiclesAsync();
        return Ok(vehicles);
    }

    [HttpGet("{characterId:int}")]
    public async Task<IActionResult> GetVehiclesByCharacterId([FromRoute] int characterId)
    {
        VehicleDetail? detail = await _vehicleService.GetVehicleByCharacterIdAsync(characterId);

        return detail is not null
        ? Ok(detail)
        : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateVehicleById([FromBody] VehicleUpdate request)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _vehicleService.UpdateVehicleAsync(request)
            ? Ok("Vehicle was updated successfully!")
            : BadRequest("Please try again.");
    }

    [HttpDelete("{vehicleId:int}")]
    public async Task<IActionResult> DeleteVehicle([FromBody] int vehicleId)
    {
        return await _vehicleService.DeleteVehicleAsync(vehicleId)
            ? Ok($"Vehicle: {vehicleId} was successfully deleted!")
            : BadRequest("Please try again.");
    }
}
