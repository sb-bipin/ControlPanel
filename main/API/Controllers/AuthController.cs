using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Responses;
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

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var userId = await _identityService.RegisterAsync(dto.Username, dto.Email, dto.Password, dto.FirstName, dto.LastName);
                return Ok(ServiceResponse<string>.SuccessResponse(userId, "Registration successful."));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ServiceResponse<string>.ExceptionResponse(ex));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<LogInResponse>>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var response = await _identityService.LoginAsync(dto.Email, dto.Password);
                return Ok(ServiceResponse<LogInResponse>.SuccessResponse((LogInResponse)response, "Login successful."));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.Unauthorized, ServiceResponse<LogInResponse>.FailureResponse(ex.Message, "Login failed.", (int)System.Net.HttpStatusCode.Unauthorized));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<IApplicationUser>>>> GetAllUsers()
        {
            try
            {
                var users = await _identityService.GetAllUsersAsync();
                return Ok(ServiceResponse<List<IApplicationUser>>.SuccessResponse(users.ToList(), "Users retrieved."));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ServiceResponse<List<IApplicationUser>>.ExceptionResponse(ex));
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                var token = await _identityService.GeneratePasswordResetTokenAsync(dto.Email);
                return Ok(ServiceResponse<string>.SuccessResponse(token, "Password reset token generated."));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ServiceResponse<string>.ExceptionResponse(ex));
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                var result = await _identityService.ResetPasswordAsync(dto.UserId, dto.NewPassword);
                if (!result)
                    return BadRequest(ServiceResponse<string>.FailureResponse("Failed to reset password.", "Password reset failed."));

                return Ok(ServiceResponse<string>.SuccessResponse("", "Password reset successful."));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ServiceResponse<string>.ExceptionResponse(ex));
            }
        }
    }


}