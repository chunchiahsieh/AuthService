using Api.Authorization;
using Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);
// 讀取 Connection String 並註冊 DbContext
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration); // JWT設定

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger(); // Swagger設定
builder.Services.AddCustomAuthorization(); // 克制化權限管理

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();//強制轉換HTTP to HTTPS

app.UseAuthentication();//啟用身份驗證 [Authorize]表示有token才能使用
app.UseAuthorization(); //啟用授權中介軟體

app.MapControllers();

app.Run();
