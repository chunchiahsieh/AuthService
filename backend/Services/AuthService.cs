using Api.Controllers;
using Api.Data;
using Api.DTOs;
using Api.Models;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Services
{
    public class AuthService:IAuthService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        public AuthService(ApiDbContext context,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(string AccessToken, string RefreshToken)> Login(LoginRequest dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            var token = await _context.AuthTokens.FirstOrDefaultAsync(u => u.UserId == user.Id);
            if (token != null)
            {
                return (token.AccessToken, token.RefreshToken);
            }
               
            var accessToken = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshToken();
            // 儲存 Token 到資料庫
            var authToken = new AuthToken
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1), // Access Token 過期時間
                CreatedAt = DateTime.UtcNow
            };
            _context.AuthTokens.Add(authToken);
            await _context.SaveChangesAsync();
            return (accessToken, refreshToken);
        }

        public async Task<string> GenerateRefreshToken()
        {
            string refreshToken;
            bool exists;

            do
            {
                refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                exists = await _context.AuthTokens.AnyAsync(t => t.RefreshToken == refreshToken);
            }
            while (exists); // 如果已經存在，則重新生成

            return refreshToken;
        }

        private string GenerateJwtToken(Api.Models.User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "DefaultSecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 添加 SystemName，這樣 JWT 就能區分不同系統的使用者
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("userid", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT 唯一識別碼
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }


        public async Task<bool> Logout(Guid userId)
        {
            var token = await _context.AuthTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (token == null) return false;

            _context.AuthTokens.Remove(token);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshToken(string refreshToken)
        {
            var storedToken = await _context.AuthTokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var user = await _context.Users.FindAsync(storedToken.UserId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // 產生新的 Access Token & Refresh Token
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = await GenerateRefreshToken();

            // 刪除舊的 Refresh Token
            _context.AuthTokens.Remove(storedToken);

            // 建立新的 Refresh Token 紀錄
            var newTokenRecord = new AuthToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // 例如 7 天後過期
                CreatedAt = DateTime.UtcNow
            };

            _context.AuthTokens.Add(newTokenRecord);
            await _context.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }

        public async Task<(string message,User user)> AutoRegister()
        {

            var userCount = await _context.Users.CountAsync();
            if (userCount > 0)
            {
                // 如果有資料，則不允許註冊，並返回錯誤訊息
                return (message:"Cannot register users because the Users table is not empty", user:new User());
            }

            // 創建新的用戶實例
            var newUser = new User
            {
                Email = "admin",
                PasswordHash = HashPassword("1111"), // 密碼處理方法應使用強加密算法
            };

            // 將新用戶加入資料庫
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            newUser.PasswordHash = "1111";
            return (message:"User successfully registered.", user:newUser);
        }
    }
}
