using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public IdentityService(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<List<IApplicationUser>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                                .Select(user => new ApplicationUserDto
                                {
                                    Id = user.Id,
                                    Email = user.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Username = user.UserName,
                                    CreatedBy = user.CreatedBy,
                                    CreatedOn = user.CreatedOn,
                                    UpdatedBy = user.UpdatedBy,
                                    UpdatedOn = user.UpdatedOn
                                })
                                .ToListAsync();


            if (users is null || !users.Any())
                return new List<IApplicationUser>();

            return users.Cast<IApplicationUser>().ToList();
        }
        public async Task<string> RegisterAsync(string username, string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                FirstName = firstName,
                LastName = lastName,
                CreatedBy = new Guid(),
                CreatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

            return user.Id;
        }

        public async Task<ILogInResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new Exception("Username not found for login.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, password, false, false);

            if (!result.Succeeded)
                throw new Exception("Invalid login attempt.");

            var token = _tokenService.GenerateJwtToken(user);

            return new LogInResponse
            {
                Token = token
            };
        }

        public async Task<bool> AddToRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
    }
}