using Domain.Exceptions;
using Domain.Exceptions.Auth;
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
        IOptions<JwtOptions> options,
        RoleManager<IdentityRole> roleManager
        ) : IAuthService
    {
        

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) throw new UserNotFoundException(loginDto.UserName);

            var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!flag) throw new UnAuthorizedException();

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Role = role,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null) throw new ArgumentNullException(nameof(registerDto));
            if (userManager == null) throw new InvalidOperationException("UserManager is not configured.");
            if (roleManager == null) throw new InvalidOperationException("RoleManager is not configured.");

            // Normalize inputs
            var userName = (registerDto.UserName ?? string.Empty).Trim();
            var displayName = (registerDto.DisplayName ?? string.Empty).Trim();
            var requestedRole = string.IsNullOrWhiteSpace(registerDto.Role) ? null : registerDto.Role.Trim();

            // Check username uniqueness before creating
            var existing = await userManager.FindByNameAsync(userName);
            if (existing != null)
                throw new ValidationException(new[] { "UserName already exists." });

            // SECURITY: Validate the requested role exists BEFORE creating the user
            if (!string.IsNullOrEmpty(requestedRole))
            {
                var roleExists = await roleManager.RoleExistsAsync(requestedRole);
                if (!roleExists)
                    throw new RoleNotFoundException(requestedRole); // or a ValidationException
            }

            var user = new AppUser
            {
                UserName = userName,
                DisplayName = displayName
            };

            // Create user
            var createResult = await userManager.CreateAsync(user, registerDto.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                throw new ValidationException(errors);
            }

            // If a role was requested, try to assign it.
            if (!string.IsNullOrEmpty(requestedRole))
            {
                var roleAssignResult = await userManager.AddToRoleAsync(user, requestedRole);
                if (!roleAssignResult.Succeeded)
                {
                    // Cleanup: try to delete the created user to avoid orphan accounts
                    try
                    {
                        await userManager.DeleteAsync(user);
                    }
                    catch(Exception ex) 
                    {
                        
                    }

                    var errors = roleAssignResult.Errors.Select(e => e.Description);
                    throw new ValidationException(errors);
                }
            }

            // Get the assigned role(s) from the store (don't trust client input)
            var roles = await userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault(); // أو رجّع List<string> لو عايز كل الأدوار

            // Generate token (GenerateJwtTokenAsync already reads roles via userManager.GetRolesAsync inside)
            var token = await GenerateJwtTokenAsync(user);

            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Role = primaryRole,
                Token = token
            };
        }

        public async Task<UserProfileDto> GetCurrentUserAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                throw new UserNotFoundException(userName);

            var roles = await userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault();

            return new UserProfileDto
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Role = primaryRole
            };
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUsersAsync()
        {
            var users = userManager.Users.ToList();
            var result = new List<UserProfileDto>();

            foreach(var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                result.Add(new UserProfileDto 
                {
                    UserName = user.UserName,
                    DisplayName= user.DisplayName,
                    Role = roles.FirstOrDefault()
                });
            }

            return result;
            
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await userManager.FindByNameAsync(changePasswordDto.UserName);
            if (user == null)
                throw new UserNotFoundException(changePasswordDto.UserName);

            var result = await userManager.ChangePasswordAsync(
                user,
                changePasswordDto.CurrrentPassword,
                changePasswordDto.NewPassword
                );

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new ValidationException(errors);

            }

            return true;
        }
        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            //Header
            //Payload
            //Signature
            var jwtOptions = options.Value;

            var authClaims = new List<Claim>()
            {
                new Claim("user_name", user.UserName),
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                authClaims.Add(item: new Claim("role", role));
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
