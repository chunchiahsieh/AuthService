using Api.DTOs;

namespace Api.Services
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> Login(LoginDTO dto);
        Task<bool> Logout(Guid userId);
        Task<(string AccessToken, string RefreshToken)> RefreshToken(string refreshToken);
    }
}
