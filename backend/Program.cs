using Api.Authorization;
using Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration); // Swagger設定

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger(); // Swagger設定
builder.Services.AddCustomAuthorization(); // 加入自訂授權

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();//讓 所有 HTTP 請求自動轉向 HTTPS，提高安全性。

app.UseAuthentication();//使用權限確保API有用
app.UseAuthorization(); //讓token授權機制生效

app.MapControllers();

app.Run();
