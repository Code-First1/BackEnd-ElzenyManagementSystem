using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Shared.DTOs.Auth;
using Shared.DTOs.Product;
using Shared.DTOs.User;
using Shared.ErrorModels;
using Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route(template:"api/[controller]")]
    public class AuthController(IServiceManager serviceManager) :ControllerBase
    {
        //login
        [HttpPost(template:"login")] //POST /api/auth/login
        [AllowAnonymous]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status200OK, Type = typeof(UserResultDto))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var resul = await serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(resul);

        }

        //register
        [HttpPost(template: "register")] //POST /api/auth/register
        [AllowAnonymous]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status200OK, Type = typeof(UserResultDto))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await serviceManager.AuthService.ChangePasswordAsync(changePasswordDto);
            return NoContent();
        }

        [HttpGet("GetProfile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserProfileDto>> GetCurrentUserJ()
        {
            var userName = User.FindFirstValue("user_name");
            if(string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await serviceManager.AuthService.GetCurrentUserAsync(userName);
            return Ok(user);
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<UserProfileDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllUsers()
        {
            var users = await serviceManager.AuthService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]           
        [ProducesResponseType(StatusCodes.Status404NotFound)]   
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var result = await serviceManager.AuthService.DeleteUserAsync(username);
            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User deleted successfully" });
        }

    }
}
