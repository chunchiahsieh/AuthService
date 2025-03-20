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
