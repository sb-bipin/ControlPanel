
namespace Application.Common.Models
{
    public interface ILogInResponse
    {
        string UserId { get; }
        string Token { get; }
    }
}
