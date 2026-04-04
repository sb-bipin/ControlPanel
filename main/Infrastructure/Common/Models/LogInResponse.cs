using Application.Common.Models;

namespace Infrastructure.Common.Models
{
    public class LogInResponse : ILogInResponse
    {
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}