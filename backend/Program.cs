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
builder.Services.AddJwtAuthentication(builder.Configuration); // Swagger�]�w

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger(); // Swagger�]�w
builder.Services.AddCustomAuthorization(); // �[�J�ۭq���v

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();//�� �Ҧ� HTTP �ШD�۰���V HTTPS�A�����w���ʡC

app.UseAuthentication();//�ϥ��v���T�OAPI����
app.UseAuthorization(); //��token���v����ͮ�

app.MapControllers();

app.Run();
