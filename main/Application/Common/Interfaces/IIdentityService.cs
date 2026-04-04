using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> RegisterAsync(string username, string email, string password, string firstName, string lastName);
        Task<ILogInResponse> LoginAsync(string email, string password);
        Task<bool> AddToRoleAsync(string userId, string role);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<List<IApplicationUser>> GetAllUsersAsync();
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string userId, string newPassword);
    }
}
