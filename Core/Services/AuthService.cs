using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared.DTOs.Auth;
using Shared.DTOs.User;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService(
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> options
        ) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);
            if (userManager is null) throw new UnAuthorizedException();

            var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!flag) throw new UnAuthorizedException();

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(errors => errors.Description);
                throw new ValidationException(errors);
            }

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            //Header
            //Payload
            //Signature
            var jwtOptions = options.Value;

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                authClaims.Add(item: new Claim(ClaimTypes.Role, role));
            }

            // "dssdsdsdsd"
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddHours(jwtOptions.DurationInHours),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                );

            //Token

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
