using Api.DTOs;
using Api.Models;

namespace Api.Services
{
    public interface IAuthService
    {
        Task<(string message,User user)> AutoRegister();
        Task<(string AccessToken, string RefreshToken)> Login(LoginDTO dto);
        Task<bool> Logout(Guid userId);
        Task<(string AccessToken, string RefreshToken)> RefreshToken(string refreshToken);
    }
}
