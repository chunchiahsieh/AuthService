using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Data;
using Api.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly Api.Services.IAuthService _service;
        private readonly IWebHostEnvironment _env;

        public LoginController(Api.Services.IAuthService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
    }

        [AllowAnonymous]//註冊帳號測試用
        [HttpPost("AutoRegister")]
        public async Task<IActionResult> AutoRegister()
        {
            try
            {
                var (message,user) = await _service.AutoRegister(); // 讓 AuthService 處理登入邏輯
                return Ok(new { message, user }); // 正確的返回格式，使用匿名對象
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {

            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            try
            {
                var tokens = await _service.Login(model); // 讓 AuthService 處理登入邏輯
                return Ok(new
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var tokens = await _service.RefreshToken(dto.RefreshToken);
                return Ok(new
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }



    }
}
