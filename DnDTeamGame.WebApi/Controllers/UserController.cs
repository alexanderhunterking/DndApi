using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnDTeamGame.Services.UserServices;
using DnDTeamGame.Services.TokenServices;
using DnDTeamGame.Models.UserModels;
using DnDTeamGame.Models.TokenModels;
using DnDTeamGame.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DnDTeamGame.WebApi.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
           try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var registerResult = await _userService.RegisterUserAsync(model);
                if (registerResult)
                {
                    TextResponse response = new("User was registered.");
                    return Ok(response);
                }
                return BadRequest(new TextResponse("User could not be registered."));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new TextResponse("User could not be registered."));
            }
        }

        // [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int userId)
        {
            UserDetail? detail = await _userService.GetUserByIdAsync(userId);

            if (detail is null)
            {
                return NotFound();
            }

            return Ok(detail);
        }
        [Authorize]
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            return await _userService.DeleteUserAsync(userId)
                ? Ok($"User with the Id: {userId} was deleted successfully.")
                : BadRequest($"User with the Id: {userId} could not be deleted.");
        }

        [HttpPost("~/api/Token")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TokenResponse? response = await _tokenService.GetTokenAsync(request);

            if (response is null)
            {
                return BadRequest(new TextResponse("Invalid Username or Password."));
            }

            return Ok(response);
        }


    }
}
