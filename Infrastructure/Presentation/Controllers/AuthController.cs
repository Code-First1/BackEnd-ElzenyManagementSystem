using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.Auth;
using Shared.DTOs.Product;
using Shared.DTOs.User;
using Shared.ErrorModels;
using Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType<UserResultDto>(StatusCodes.Status200OK, Type = typeof(UserResultDto))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(result);
        }
        //register
        [HttpPost(template: "register")] //POST /api/auth/register
        [ProducesResponseType<UserResultDto>(StatusCodes.Status200OK, Type = typeof(UserResultDto))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(result);
        }
    }
}
