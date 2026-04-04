using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Identity;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var userId = await _identityService.RegisterAsync(dto.Username, dto.Email, dto.Password, dto.FirstName, dto.LastName);
            return Ok(new { UserId = userId });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LogInResponse>> Login([FromBody] LoginDto dto)
        {
            var response = await _identityService.LoginAsync(dto.Email, dto.Password);
            return Ok(response);
        }

        [HttpGet]
        public async Task<List<IApplicationUser>> GetAllUsers()
        {
            return await _identityService.GetAllUsersAsync();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var token = await _identityService.GeneratePasswordResetTokenAsync(dto.Email);
            return Ok(new { Token = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _identityService.ResetPasswordAsync(dto.UserId, dto.NewPassword);
            if (!result)
                return BadRequest(new { Message = "Failed to reset password." });
            return Ok(new { Message = "Password reset successful." });
        }
    }


}