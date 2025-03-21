
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Api.Authorization;

namespace Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddAuthorization(options =>
            {
                //這邊加入權限檢查
                //options.AddPolicy("SwaggerPolicy", policy => policy.RequireRole("superUser"));
            }
            );

            return services;
        }
    }
}
