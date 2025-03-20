using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // JWT 驗證設定
                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "請輸入 JWT Token（格式: Bearer {token}）",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new List<string>() }
                });
            });

            return services;
        }
    }
}
